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
using System.Globalization;

namespace Chioy.Communication.Networking.Client.FTP.Helper
{
    /// <summary>
    /// FTP servers return different dateFormats, depending on 
    /// OS (Windows,Unix..), Culture(USA,Germany..) 
    /// library is tested on Unix(english) Windows(IIS7-german, Filezilla-german)
    /// if library is not able to parse records please send me an example
    /// egon.duerr@gmx.at
    /// </summary>
    internal static class ConvertDate
    {
        internal static bool Parse( string date,string time, out DateTime? dateTime )
        {
            dateTime = null;
            DateTime dateTimeTmp;
            var success = false;
            DateTime.TryParse( date + " " + time, out dateTimeTmp );
            foreach ( var cultureInfo in _cultureInfos )
            {
                if ( DateTime.TryParse( date + " " + time, out dateTimeTmp ) )
                {
                    dateTime = dateTimeTmp;
                    success = true;
                    break;
                }//if
            }//foreach
            
            if ( !success )
            {
                try
                {
                    // could be done in one line
                    // using two lines makes it easy to find exception in parsing, in most cases only one part fails  
                    var dateTimeTmp1 = DateTime.ParseExact( time, "hh:mmtt", CultureInfo.InvariantCulture );
                    var dateTimeTmp2 = DateTime.ParseExact( date, "MM-dd-yyyy", CultureInfo.InvariantCulture );
                    var ddd = dateTimeTmp2.ToShortDateString() + " " + dateTimeTmp1.TimeOfDay;
                    dateTime = DateTime.Parse( dateTimeTmp2.ToShortDateString() + " " + dateTimeTmp1.TimeOfDay, CultureInfo.CurrentCulture );
                    success = true;
                }//if
                catch { }
            }//if
            return success;        
        }//method

        private static List<CultureInfo> _cultureInfos = new List<CultureInfo>()
        {
            CultureInfo.InvariantCulture,
            new CultureInfo("en-US"),
            new CultureInfo("de-DE"),
            new CultureInfo("fr-FR"),
            new CultureInfo( "ja-JP")
        };
    
    }
}
