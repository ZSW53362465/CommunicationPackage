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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Chioy.Communication.Networking.Client.FTP.Helper;

namespace Chioy.Communication.Networking.Client.FTP
{
    /// <summary>
    /// Basic class of DotNetFtpLibrary
    /// </summary>
    public class FtpClient
    {
        private const int BUFFER_SIZE = 32768;
        //private const int BUFFER_SIZE = 1024;
        private Thread _thread;
        private bool _abort = false;
        private string _host;
        
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Port | defaultValue = 21
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// URL of your ftpServer e.g. 192.168.0.1, www.myftpserver.com ! DO NOT USE: ftp://192.168.0.1, ftp://www.myftpserver.com
        /// </summary>
        public string Host { get { return _host; } set { this.SetHost( value ); } }
        /// <summary>
        /// Transfermode | defaultValue = true | usePassive = false maybe fails (firewall settings..)
        /// </summary>
        public bool UsePassive { get; set; }
        /// <summary>
        /// Timeout of connection | defaultValue = 30000
        /// </summary>
        public int TimeOut { get; set; }
        /// <summary>
        /// KeepAlive | defaultValue = false
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// Occurs when upload process has changed
        /// </summary>
        public event EventHandler<UploadProgressChangedLibArgs> UploadProgressChanged;
        /// <summary>
        /// Occurs when upload process has completed or error has been detected
        /// </summary>
        public event EventHandler<UploadFileCompletedEventLibArgs> UploadFileCompleted;
        /// <summary>
        /// Occurs when download process has changed
        /// </summary>
        public event EventHandler<DownloadProgressChangedLibArgs> DownloadProgressChanged;
        /// <summary>
        /// Occurs when download process has completed or error has been detected
        /// </summary>
        public event EventHandler<DownloadFileCompletedEventLibArgs> DownloadFileCompleted;
        
        /// <summary>
        /// Creates an instance of FtpClient
        /// </summary>
        public FtpClient()
        {
            UsePassive = true;
            this.TimeOut = 30000;
            KeepAlive = false;
        }//constructor

        /// <summary>
        /// Creates an instance of FtpClient
        /// </summary>
        /// <param name="host">URL of your ftpServer e.g. 192.168.0.1, www.myftpserver.com ! DO NOT USE: ftp://192.168.0.1, ftp://www.myftpserver.com</param>
        /// <param name="userName">UserName</param>
        /// <param name="password">Password</param>
        /// <param name="port">Port of ftpServer | defaultValue = 21</param>
        public FtpClient( string host, string userName, string password, int port )
        {
            Host = host;
            UserName = userName;
            Password = password;
            Port = port;
            UsePassive = true;
            this.TimeOut = 30000;
            KeepAlive = false;
        }//constructor

        /// <summary>
        /// Creates an instance of FtpClient
        /// </summary>
        /// <param name="host">URL of your ftpServer e.g. 192.168.0.1, www.myftpserver.com ! DO NOT USE: ftp://192.168.0.1, ftp://www.myftpserver.com</param>
        /// <param name="userName">UserName</param>
        /// <param name="password">Password</param>
        /// <param name="port">Port of ftpServer | defaultValue = 21</param>
        /// <param name="usePassive">Transfermode | defaultValue = true | usePassive = false maybe fails (firewall settings..)</param>
        public FtpClient( string host, string userName, string password, int port, bool usePassive )
        {
            Host = host;
            UserName = userName;
            Password = password;
            Port = port;
            UsePassive = usePassive;
            this.TimeOut = 30000;
            KeepAlive = false;
        }//constructor


        /// <summary>
        /// Uploads file to ftpServer | method blocks calling thread till upload is finnished or error occured
        /// </summary>
        /// <param name="localDirectory">directory on localhost</param>
        /// <param name="localFilename">filename on localhost</param>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="remoteFileName">filename on ftpServer</param>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public void Upload(string localDirectory, string localFilename, string remoteDirectory, string remoteFileName)
        {
            Upload(Path.Combine(localDirectory, localFilename), remoteDirectory, remoteFileName);
        }// method

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Upload(string localFullFileName, string remoteDirectory, string remoteFileName)
        {
            _abort = false;
            FtpWebRequest request = null;
            long totalBytesSend = 0;

            try
            {
                request = WebRequest.Create(new Uri("ftp://" + _host + ":" + Port + "/" + remoteDirectory + "/" + remoteFileName)) as FtpWebRequest;
                request.Credentials = new NetworkCredential(UserName, Password);
                request.UsePassive = UsePassive;
                request.Timeout = TimeOut;
                request.KeepAlive = KeepAlive;
                WebException webException;
                Debug.WriteLine("proofing directory exists");
                if (!this.DirectoryExits(remoteDirectory, out webException))
                {
                    this.CreateDirectoryRecursive(remoteDirectory, out webException);

                }//if
                request.Method = WebRequestMethods.Ftp.UploadFile;

                //request.ContentLength = fi.Length - remoteFileSize;

                Debug.WriteLine("starting upload");
                using (Stream requestStream = request.GetRequestStream())
                {
                    using (FileStream fs = File.Open(localFullFileName, FileMode.Open))
                    {
                        fs.Seek(0, SeekOrigin.Begin);
                        byte[] buffer = new byte[BUFFER_SIZE];
                        int readBytes = 0;
                        do
                        {
                            readBytes = fs.Read(buffer, 0, BUFFER_SIZE);
                            requestStream.Write(buffer, 0, readBytes);
                            if (UploadProgressChanged != null && !_abort)
                            {
                                UploadProgressChanged(this, new UploadProgressChangedLibArgs(fs.Position, fs.Length));
                            }
                            //System.Threading.Thread.Sleep(500);
                        } while (readBytes != 0 && !_abort);
                        totalBytesSend = fs.Length;
                    }//using
                }//using
                if (UploadFileCompleted != null && !_abort)
                {
                    var uploadFileCompleteArgs = new UploadFileCompletedEventLibArgs(totalBytesSend, TransmissionState.Success);
                    UploadFileCompleted(this, uploadFileCompleteArgs);
                }//if
            }//try
            catch (WebException webException)
            {
                if (UploadFileCompleted != null && !_abort)
                {
                    UploadFileCompleted(this, new UploadFileCompletedEventLibArgs(totalBytesSend, TransmissionState.Failed, webException));
                }//if
            }//catch
            catch (Exception exp)
            {
                var webException = exp as WebException;
                if (UploadFileCompleted != null && !_abort)
                {
                    UploadFileCompleted(this, new UploadFileCompletedEventLibArgs(totalBytesSend, TransmissionState.Failed, webException));
                }//if
            }//catch
        }// method

        /// <summary>
        /// Method uploads file to ftpServer | method does not block calling thread 
        /// | file exists on FtpServer => method overrides file
        /// | directory does not exist on FtpServer => directory is created
        /// </summary>
        /// <param name="localDirectory">directory on localhost</param>
        /// <param name="localFilename">filename on localhost</param>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="remoteFileName">filename on ftpServer</param>
        public void UploadAsync( string localDirectory, string localFilename, string remoteDirectory, string remoteFileName )
        {
            ThreadParameters parameters = new ThreadParameters( localDirectory, localFilename, remoteDirectory, remoteFileName );
            ParameterizedThreadStart pThreadStart = new ParameterizedThreadStart( this.DoUploadAsync );
            _thread = new Thread( pThreadStart );
            _thread.Name = "UploadThread";
            _thread.IsBackground = true;
            _thread.Priority = ThreadPriority.Normal;
            _thread.Start( parameters );
        }// method

        /// <summary>
        /// Method uploads file to FtpServer 
        /// | file exists on FtpServer => method resumes upload (apends file)
        /// | file does not exist on FtpServer => method creates file
        /// | directory does not exist on FtpServer => directory is created
        /// </summary>
        /// <param name="localDirectory">directory on localhost</param>
        /// <param name="localFilename">filename on localhost</param>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="remoteFileName">filename on ftpServer</param>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public void UploadResume( string localDirectory, string localFilename, string remoteDirectory, string remoteFileName )
        {
            _abort = false;
            FileInfo fi = new FileInfo(Path.Combine(localDirectory, localFilename));
            long remoteFileSize = 0;
            FtpWebRequest request = null;
            long totalBytesSend = 0;

            request = WebRequest.Create( new Uri( "ftp://" + _host + ":" + Port + "/" + remoteDirectory + "/" + remoteFileName ) ) as FtpWebRequest;
            request.Credentials = new NetworkCredential( UserName, Password );
            request.Timeout = TimeOut;
            request.UsePassive = UsePassive;
            request.KeepAlive = KeepAlive;
            try
            {
                if ( this.FileExists( remoteDirectory, remoteFileName, out remoteFileSize ) )
                {
                    request.Method = WebRequestMethods.Ftp.AppendFile;
                }
                else
                {
                    WebException webException;
                    if ( ! this.DirectoryExits( remoteDirectory, out webException ) )
                    {
                        var directoryCreated = this.CreateDirectory( remoteDirectory, out webException );
                    }//if
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                }
                request.ContentLength = fi.Length - remoteFileSize;
                request.UsePassive = true ;

                using ( Stream requestStream = request.GetRequestStream() )
                {
                    using ( FileStream logFileStream = new FileStream( Path.Combine( localDirectory, localFilename ), FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )                    
                    {
                        StreamReader fs = new StreamReader( logFileStream );

                        fs.BaseStream.Seek( remoteFileSize, SeekOrigin.Begin );
                        byte[] buffer = new byte[ BUFFER_SIZE ];
                        int readBytes = 0;
                        do
                        {
                            readBytes = fs.BaseStream.Read( buffer, 0, BUFFER_SIZE );
                            requestStream.Write( buffer, 0, readBytes );
                            if (UploadProgressChanged != null && !_abort)
                            {
                                UploadProgressChanged( this, new UploadProgressChangedLibArgs( fs.BaseStream.Position, fs.BaseStream.Length ) );
                            }
                            //System.Threading.Thread.Sleep(500);
                        } while (readBytes != 0 && !_abort);
                        //_fileStreams.Remove( fs );
                        requestStream.Close();
                        totalBytesSend = fs.BaseStream.Length;
                        fs.Close();
                        logFileStream.Close();
                        Thread.Sleep( 100 );
                    }//using
                }//using
                //Console.WriteLine( "Done" );
                if ( UploadFileCompleted != null && !_abort )
                {
                    var uploadFileCompleteArgs = new UploadFileCompletedEventLibArgs( totalBytesSend, TransmissionState.Success );
                    UploadFileCompleted( this, uploadFileCompleteArgs );
                }//if
            }//try
            catch ( WebException webException )
            {
                if (UploadFileCompleted != null && !_abort)
                { 
                    UploadFileCompleted( this, new UploadFileCompletedEventLibArgs( totalBytesSend, TransmissionState.Failed, webException ) );
                }//if
            }//catch
            catch ( Exception exp )
            {
                if (UploadFileCompleted != null && !_abort)
                {
                    UploadFileCompleted( this, new UploadFileCompletedEventLibArgs( totalBytesSend, TransmissionState.Failed, exp ) );
                }//if
            }//catch

        }//method

        /// <summary>
        /// Method uploads file to FtpServer
        /// method does not block calling thread
        ///  file exists on FtpServer => method resumes upload
        ///  file does not exist on FtpServer => method creates file
        /// </summary>
        /// <param name="localDirectory">directory on localhost</param>
        /// <param name="localFilename">filename on localhost</param>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="remoteFileName">filename on ftpServer</param>
        public void UploadResumeAsync( string localDirectory, string localFilename, string remoteDirectory, string remoteFileName )
        {
            ThreadParameters parameters = new ThreadParameters( localDirectory, localFilename, remoteDirectory, remoteFileName );
            ParameterizedThreadStart pThreadStart = new ParameterizedThreadStart( this.DoUploadResumeAsync );
            _thread = new Thread( pThreadStart );
            _thread.Name = "UploadThread";
            _thread.IsBackground = true;
            _thread.Priority = ThreadPriority.Normal;
            _thread.Start( parameters );
        }//method

        /// <summary>
        /// Method downloads file to FtpServer
        /// Method blocks calling thread
        ///  file to download exits on localhost => file is overwritten
        ///  file does not exist on localhost => file is created
        /// </summary>
        /// <param name="localDirectory">directory on localhost</param>
        /// <param name="localFilename">filename on localhost</param>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="remoteFileName">filename on ftpServer</param>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public void Download(string localDirectory, string localFilename, string remoteDirectory, string remoteFileName)
        {
            _abort = false;
            var localFile = Path.Combine(localDirectory, localFilename);
            FileInfo file = new FileInfo( localFile );
            FileStream localfileStream = null;
            long totalBytesReceived = 0;
            try
            {
                FtpWebRequest request = FtpWebRequest.Create( new Uri( "ftp://" + _host + ":" + Port + "/" + remoteDirectory + "/" + remoteFileName ) ) as FtpWebRequest;
                request.Credentials = new NetworkCredential( UserName, Password );
                request.UsePassive = UsePassive;
                request.Timeout = TimeOut;
                request.KeepAlive = KeepAlive;
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                long remoteFileSize = this.GetFileSize( remoteDirectory, remoteFileName );
                localfileStream = new FileStream( localFile, FileMode.Create, FileAccess.Write );

                FtpWebResponse response = request.GetResponse() as FtpWebResponse;
                using ( Stream ftpStream = response.GetResponseStream() )
                {
                    byte[] buffer = new byte[ BUFFER_SIZE ];
                    int bytesRead = ftpStream.Read( buffer, 0, BUFFER_SIZE );
                    totalBytesReceived = bytesRead;
                    while (bytesRead != 0 && !_abort)
                    {
                        localfileStream.Write( buffer, 0, bytesRead );
                        bytesRead = ftpStream.Read( buffer, 0, BUFFER_SIZE );
                        totalBytesReceived += bytesRead;
                        if (DownloadProgressChanged != null && !_abort)
                        {
                            DownloadProgressChanged( this, new DownloadProgressChangedLibArgs( totalBytesReceived, remoteFileSize ) );
                        }//if
                    }//while
                    localfileStream.Close();
                }//using
                if (DownloadFileCompleted != null && !_abort)
                {
                    var downloadFileCompleteArgs = new DownloadFileCompletedEventLibArgs( totalBytesReceived, TransmissionState.Success );
                    DownloadFileCompleted( this, downloadFileCompleteArgs );
                }//if

            }//try
            catch ( WebException webException )
            {
                if ( DownloadFileCompleted != null && !_abort )
                {
                    DownloadFileCompleted( this, new DownloadFileCompletedEventLibArgs( totalBytesReceived, TransmissionState.Failed, webException ) );
                }//if
            }//catch
            catch ( Exception exp )
            {
                var webException = exp as WebException;
                if (DownloadFileCompleted != null && !_abort )
                {
                    DownloadFileCompleted( this, new DownloadFileCompletedEventLibArgs( totalBytesReceived, TransmissionState.Failed, webException ) );
                }//if
            }//catch
            finally
            {
                if ( localfileStream != null ) localfileStream.Close();
            }
        }//method

        /// <summary>
        /// Method downloads file to FtpServer
        /// Method does not block calling thread
        ///  file to download exits on localhost => file is overwritten
        ///  file does not exist on localhost => file is created
        /// </summary>
        /// <param name="localDirectory">directory on localhost</param>
        /// <param name="localFilename">filename on localhost</param>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="remoteFileName">filename on ftpServer</param>
        public void DownloadAsync( string localDirectory, string localFilename, string remoteDirectory, string remoteFileName )
        {
            ThreadParameters parameters = new ThreadParameters( localDirectory, localFilename, remoteDirectory, remoteFileName );
            ParameterizedThreadStart pThreadStart = new ParameterizedThreadStart( this.DoDownloadAsync );
            _thread = new Thread( pThreadStart );
            _thread.Name = "DownloadThread";
            _thread.IsBackground = true;
            _thread.Priority = ThreadPriority.Normal;
            _thread.Start( parameters );
        }// method

        /// <summary>
        /// Method downloads file to FtpServer
        /// Method blocks calling thread
        ///  file to download exits on localhost => file is appended
        ///  file does not exist on localhost => file is created
        /// </summary>
        /// <param name="localDirectory">directory on localhost</param>
        /// <param name="localFilename">filename on localhost</param>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="remoteFileName">filename on ftpServer</param>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public void DownloadResume(string localDirectory, string localFilename, string remoteDirectory, string remoteFileName)
        {
            _abort = false;
            var localFile = Path.Combine( localDirectory, localFilename );
            FileInfo file = new FileInfo( localFile );
            FileStream localfileStream = null;
            long totalBytesReceived = 0;
            long localFileSize = 0;
            try
            {
                FtpWebRequest request = FtpWebRequest.Create( new Uri( "ftp://" + _host + ":" + Port + "/" + remoteDirectory + "/" + remoteFileName ) ) as FtpWebRequest;
                request.Credentials = new NetworkCredential( UserName, Password );
                request.UsePassive = UsePassive;
                request.Timeout = TimeOut;
                request.KeepAlive = KeepAlive;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                long remoteFileSize = this.GetFileSize( remoteDirectory, remoteFileName );
                if ( file.Exists )
                {
                    if ( file.Length == remoteFileSize )
                    {
                        if ( DownloadFileCompleted != null )
                        {
                            var downloadFileCompleteArgs = new DownloadFileCompletedEventLibArgs( 0, TransmissionState.Success );
                            DownloadFileCompleted( this, downloadFileCompleteArgs );
                            return;
                        }//if
                    }//if
                    else if ( file.Length > remoteFileSize )
                    {
                        if ( DownloadFileCompleted != null )
                        {
                            var downloadFileCompleteArgs = new DownloadFileCompletedEventLibArgs( 0, TransmissionState.LocalFileBiggerAsRemoteFile );
                            DownloadFileCompleted( this, downloadFileCompleteArgs );
                            return;
                        }//if
                    }//else if
                    else
                    {
                        localfileStream = new FileStream( localFile, FileMode.Append, FileAccess.Write );
                        request.ContentOffset = file.Length;
                        localFileSize = file.Length;
                    }//else
                }
                else
                {
                    localfileStream = new FileStream( localFile, FileMode.Create, FileAccess.Write );
                }
                FtpWebResponse response = request.GetResponse() as FtpWebResponse;
                using ( Stream ftpStream = response.GetResponseStream() )
                {
                    byte[] buffer = new byte[ BUFFER_SIZE ];
                    int bytesRead = ftpStream.Read( buffer, 0, BUFFER_SIZE );
                    totalBytesReceived = localFileSize + bytesRead;
                    while (bytesRead != 0 && !_abort )
                    {
                        localfileStream.Write( buffer, 0, bytesRead );
                        bytesRead = ftpStream.Read( buffer, 0, BUFFER_SIZE );
                        totalBytesReceived += bytesRead;
                        if ( DownloadProgressChanged != null && !_abort )
                        {
                            DownloadProgressChanged( this, new DownloadProgressChangedLibArgs( totalBytesReceived, remoteFileSize ) );
                        }//if
                    }//while
                    localfileStream.Close();
                }//using
                if (DownloadFileCompleted != null && !_abort)
                {
                    var downloadFileCompleteArgs = new DownloadFileCompletedEventLibArgs( totalBytesReceived, TransmissionState.Success );
                    DownloadFileCompleted( this, downloadFileCompleteArgs );
                }//if

            }//try
            catch ( WebException webException )
            {
                if ( DownloadFileCompleted != null && !_abort )
                {
                    DownloadFileCompleted( this, new DownloadFileCompletedEventLibArgs( totalBytesReceived, TransmissionState.Failed, webException ) );
                }//if
            }//catch
            catch ( Exception exp )
            {
                var webException = exp as WebException;
                if ( DownloadFileCompleted != null && !_abort )
                {
                    DownloadFileCompleted( this, new DownloadFileCompletedEventLibArgs( totalBytesReceived, TransmissionState.Failed, webException ) );
                }//if
            }//catch
            finally
            {
                if ( localfileStream != null ) localfileStream.Close();
            }
        }//method

        /// <summary>
        /// Method downloads file to FtpServer
        /// Method does not block calling thread
        ///  file to download exits on localhost => file is appended
        ///  file does not exist on localhost => file is created
        /// </summary>
        /// <param name="localDirectory">directory on localhost</param>
        /// <param name="localFilename">filename on localhost</param>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="remoteFileName">filename on ftpServer</param>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public void DownloadResumeAsync( string localDirectory, string localFilename, string remoteDirectory, string remoteFileName )
        {
            ThreadParameters parameters = new ThreadParameters( localDirectory, localFilename, remoteDirectory, remoteFileName );
            ParameterizedThreadStart pThreadStart = new ParameterizedThreadStart( this.DoDownloadResumeAsync );
            _thread = new Thread( pThreadStart );
            _thread.Name = "DownloadThread";
            _thread.IsBackground = true;
            _thread.Priority = ThreadPriority.Normal;
            _thread.Start( parameters );
        }//method

        /// <summary>
        /// Returns true if file exists
        /// | if file does not exist, method returns -1
        /// | dummy method of FileExists
        /// </summary>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="remoteFileName">filename on ftpServer</param>
        /// <param name="remFileSize">remote fileSize if file exists else -1</param>
        /// <returns>True if file exists else false</returns>
        public bool FileExists( string remoteDirectory, string remoteFileName, out long remFileSize )
        {
            var success = false;
            remFileSize = 0;
            var request = ( FtpWebRequest )FtpWebRequest.Create( new Uri( "ftp://" + _host + ":" + Port + "/" + remoteDirectory + "/" + remoteFileName ) ) as FtpWebRequest;
            request.Credentials = new NetworkCredential( UserName, Password );
            request.Timeout = TimeOut;
            request.UsePassive = true;
            request.KeepAlive = KeepAlive;
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            try
            {
                using ( FtpWebResponse response = request.GetResponse() as FtpWebResponse )
                {
                    using ( StreamReader sr = new StreamReader( response.GetResponseStream(), System.Text.Encoding.ASCII ) )
                    {
                        string ftpRecord = sr.ReadToEnd();
                        response.Close();
                        string[] ftpRecords = ftpRecord.Split( '\n' );
                        var ftpListdirectoryDetails = new FtpListDirectoryDetails();
                        FileStruct fstruct = ftpListdirectoryDetails.Parse( ftpRecords[ 0 ] );
                        remFileSize = fstruct.Size;
                        success = true;
                    }//using
                }//using
            }
            catch { }
            return success;
        }//method

        /// <summary>
        /// Returns filesize
        /// | if file does not exist, method returns -1
        /// | dummy method of FileExists
        /// </summary>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="remoteFileName">filename on ftpServer</param>
        /// <returns></returns>
        public long GetFileSize( string remoteDirectory, string remoteFileName )
        {
            long remoteFilesize;
            if ( !this.FileExists( remoteDirectory, remoteFileName, out remoteFilesize ) )
            {
                remoteFilesize = -1;
            }
            return remoteFilesize;
        }//method

        /// <summary>
        ///  Creates directory recursive
        /// </summary>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="webException">WebException if creating directory fails else WebException is NULL</param>
        /// <returns>true if (sub)directories are created successfull</returns>
        public bool CreateDirectoryRecursive( string remoteDirectory, out WebException webException )
        {
            remoteDirectory = remoteDirectory.Replace( "///", "/" );
            remoteDirectory = remoteDirectory.Replace( "//", "/" );

            string[] subDirectories = remoteDirectory.Split( "/".ToArray(),StringSplitOptions.RemoveEmptyEntries );
            string subDirectory = string.Empty;
            foreach ( var subDirectoryTmp in subDirectories )
            {
                subDirectory += subDirectoryTmp;
                this.CreateDirectory( subDirectory, out webException );
                Debug.WriteLine( DateTime.Now.ToString("HH:mm:ss") );
                subDirectory += "/";
            }//foreach
            return this.DirectoryExits( subDirectory, out webException );
        }//method

        /// <summary>
        ///  Creates directory | method will not create directory tree (subdirectories)
        ///  for creating directory tree use method CreateDirectoryRecursive
        /// </summary>
        /// <param name="remoteDirectory">directory on ftpServer</param>
        /// <param name="webException">WebException if creating directory fails else WebException is NULL</param>
        /// <returns>true if directory is created successfull</returns>
        public bool CreateDirectory( string remoteDirectory, out WebException webException )
        {
            if (UploadProgressChanged != null && !_abort)
            {
                UploadProgressChanged( this, new UploadProgressChangedLibArgs( TransmissionState.CreatingDir ) );
            }//if
            webException = null;
            var success = false;
            try
            {
                var request = ( FtpWebRequest )WebRequest.Create( new Uri( "ftp://" + _host + ":" + Port + "/" + remoteDirectory ) );
                request.Credentials = new NetworkCredential( UserName, Password );
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Timeout = TimeOut;
                request.UsePassive = UsePassive;
                request.KeepAlive = KeepAlive;
                using ( FtpWebResponse response = (FtpWebResponse)request.GetResponse() )
                {
                    response.Close();
                }//using
            }//try
            catch ( WebException exp )
            {
                webException = exp;
            }//catch
            return success;
        }//method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteDirectory"></param>
        /// <param name="webException"></param>
        /// <returns></returns>
        public bool DirectoryExits( string remoteDirectory, out WebException webException )
        {
            if (UploadProgressChanged != null && !_abort)
            {
                UploadProgressChanged(this, new UploadProgressChangedLibArgs( TransmissionState.ProofingDirExits ) );
            }//if
            long remFileSize;
            webException = null;
            var success = FileExists( remoteDirectory, "", out remFileSize );
            return success;
        }//method

        /// <summary>
        /// Stopps ftp transfer
        /// </summary>
        public void Abort()
        {
            try
            {
                _abort = true;
                _thread.Abort();
            }
            catch { }
        }//method
              
        private void DoUploadResumeAsync( object threadParameters )
        {
            try
            {
                ThreadParameters p = threadParameters as ThreadParameters;
                this.UploadResume( p.LocalDirectory, p.LocalFilename, p.RemoteDirectory, p.RemoteFilename );
            }
            catch { }
        }//method

        private void DoUploadAsync( object threadParameters )
        {
            try
            {
                ThreadParameters p = threadParameters as ThreadParameters;
                this.Upload( p.LocalDirectory, p.LocalFilename, p.RemoteDirectory, p.RemoteFilename );
            }
            catch { }
        }//method

        private void DoDownloadAsync( object threadParameters )
        {
            try
            {
                ThreadParameters p = threadParameters as ThreadParameters;
                this.Download( p.LocalDirectory, p.LocalFilename, p.RemoteDirectory, p.RemoteFilename );
            }
            catch { }
        }//method

        private void DoDownloadResumeAsync( object threadParameters )
        {
            try
            {
                ThreadParameters p = threadParameters as ThreadParameters;
                this.DownloadResume( p.LocalDirectory, p.LocalFilename, p.RemoteDirectory, p.RemoteFilename );
            }
            catch { }
        }//method

        private void SetHost( string host )
        {
            _host = host;
            if ( host.ToLower().StartsWith( "ftp://" ) )
            {
                _host = _host.Substring( 6 );
            }
        }//
    }//class
}
