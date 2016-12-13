using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Client.FTP.Helper
{
    internal static class Procent_
    {

     internal static int Get( long sendBytes, long totalBytes)
     {
         int procent = 0;
         switch (totalBytes)
         { 
             case 0:
                 procent = 100;
                 break;
             default:
                 try
                 {
                     procent = (int)( (double)sendBytes / (double)totalBytes * 100d );
                 }
                 catch
                 {
                     procent = 0;
                 }
                 break;
         }//switch
         return procent;
     }//method
    
    }//class
}
