// This file is part of DotNetFtpLibrary
//
// Copyright (C) 2012, Egon Duerr, http://sourceforge.net/projects/dotnetftplib/
//
//DotNetFtpLibrary is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public
//License as published by the Free Software Foundation; either 
//version 3 of the License, or (at your option) any later version.
//
//DotNetFtpLibrary is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public 
//License along with this library.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chioy.Communication.Networking.Client.FTP.Helper
{
    /// <summary>
    /// File details of remote file/directory
    /// </summary>
    public struct FileStruct
    {
        /// <summary>
        /// FLags
        /// </summary>
        public string Flags;
        /// <summary>
        /// Owner
        /// </summary>
        public string Owner;
        /// <summary>
        /// Group
        /// </summary>
        public string Group;
        /// <summary>
        /// True if directory else false
        /// </summary>
        public bool IsDirectory;
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime;
        /// <summary>
        /// CreateTime stored in a string
        /// </summary>
        public string CreateTimeString;
        /// <summary>
        /// Name of file/directory
        /// </summary>
        public string Name;
        /// <summary>
        /// Size of file
        /// </summary>
        public long Size;
    }
    
    /// <summary>
    /// Style how ftp server lists directories
    /// </summary>
    internal enum FileListStyle
    {
        UnixStyle,
        WindowsStyle,
        Unknown
    }

    /// <summary>
    /// Provides information of directory
    /// </summary>
    public class FtpListDirectoryDetails
    {
        /// <summary>
        /// Provides information of directory
        /// </summary>
        public FtpListDirectoryDetails()
        {
        }
        
        /// <summary>
        /// Parses list of strings to list of FileStruct
        /// </summary>
        /// <param name="ftpRecords">String including information of file/directory</param>
        /// <returns>List of FileStruct</returns>
        public List<FileStruct> Parse( List<string> ftpRecords )
        {
            List<FileStruct> myListArray = new List<FileStruct>();
            //string[] dataRecords = datastring.Split( '\n' );
            //dataRecords[ 0 ] = "-rw-rw-rw- 1 user group 1171 Nov 26 00:43 blue.css\n";
            FileListStyle _directoryListStyle = GuessFileListStyle( ftpRecords );
            foreach ( string s in ftpRecords )
            {
                FileStruct f = Parse( s );
                    if ( !( f.Name == "." || f.Name == ".." ) )
                    {
                        myListArray.Add( f );
                    }
            }
            return myListArray;
        }// method

        /// <summary>
        /// Parses string to FileStruct
        /// </summary>
        /// <param name="ftpRecord">String including information of file/directory</param>
        /// <returns>FileStruct</returns>
        public FileStruct Parse( string ftpRecord )
        {
            FileStruct f = new FileStruct();
            FileListStyle _directoryListStyle = GuessFileListStyle( ftpRecord );
            if ( _directoryListStyle != FileListStyle.Unknown && ftpRecord != "" )
            {
                f.Name = "..";
                switch ( _directoryListStyle )
                {
                    case FileListStyle.UnixStyle:
                        f = ParseFileStructFromUnixStyleRecord( ftpRecord );
                        break;
                    case FileListStyle.WindowsStyle:
                        f = ParseFileStructFromWindowsStyleRecord( ftpRecord );
                        break;
                }//switch
            }//if
            return f;
        }//

        private FileStruct ParseFileStructFromWindowsStyleRecord( string Record )
        {
            //Assuming the record style as
            // 02-03-04  07:46PM       <DIR>          Append
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            string[] splitArray = processstr.Split( " \t".ToCharArray(), 2, StringSplitOptions.RemoveEmptyEntries );
            string dateStr = splitArray[ 0 ];
            processstr = splitArray[ 1 ];
            string timeStr = processstr.Substring( 0, 7 );
            processstr = ( processstr.Substring( 7, processstr.Length - 7 ) ).Trim();
            ConvertDate.Parse( dateStr, timeStr, out f.CreateTime );
            f.CreateTimeString = dateStr + timeStr;
            if ( processstr.Substring( 0, 5 ) == "<DIR>" )
            {
                f.IsDirectory = true;
                processstr = ( processstr.Substring( 5, processstr.Length - 5 ) ).Trim();
            }
            else
            {
                f.IsDirectory = false;
                string[] strs = processstr.Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
                long.TryParse( strs[ 0 ].Trim(), out f.Size ); 
                processstr = strs[ 1 ].Trim();
            }
            f.Name = processstr;  //Rest is name   
            return f;
        }

        private FileListStyle GuessFileListStyle( List<string> recordList )
        {
            foreach ( string s in recordList )
            {
                return GuessFileListStyle( s );
            }//foreach
            return FileListStyle.Unknown;
        }//method

        private FileListStyle GuessFileListStyle( string record )
        {
                if ( record.Length > 10
                 && Regex.IsMatch( record.Substring( 0, 10 ), "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)" ) )
                {
                    return FileListStyle.UnixStyle;
                }
                else if ( record.Length > 8
                 && Regex.IsMatch( record.Substring( 0, 8 ), "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]" ) )
                {
                    return FileListStyle.WindowsStyle;
                }
            return FileListStyle.Unknown;
        }//method
        
        private FileStruct ParseFileStructFromUnixStyleRecord( string Record )
        {
            //Assuming record style as
            // dr-xr-xr-x   1 owner    group               0 Nov 25  2002 bussys
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            f.Flags = processstr.Substring( 0, 9 );
            f.IsDirectory = ( f.Flags[ 0 ] == 'd' );
            processstr = ( processstr.Substring( 11 ) ).Trim();
            _cutSubstringFromStringWithTrim( ref processstr, ' ', 0 );   //skip one part
            f.Owner = _cutSubstringFromStringWithTrim( ref processstr, ' ', 0 );
            f.Group = _cutSubstringFromStringWithTrim( ref processstr, ' ', 0 );
            long.TryParse( _cutSubstringFromStringWithTrim( ref processstr, ' ', 0 ), out f.Size );   //skip one part
            f.CreateTime = DateTime.Parse( _cutSubstringFromStringWithTrim( ref processstr, ' ', 8 ) );
            f.Name = processstr;   //Rest of the part is name
            return f;
        }

        private string _cutSubstringFromStringWithTrim( ref string s, char c, int startIndex )
        {
            int pos1 = s.IndexOf( c, startIndex );
            string retString = s.Substring( 0, pos1 );
            s = ( s.Substring( pos1 ) ).Trim();
            return retString;
        }

    }//class
}