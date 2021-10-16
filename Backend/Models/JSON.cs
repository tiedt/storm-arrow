using System;

namespace SIMP.Models{

    public static class JSON{


        public static bool HasProperty(Object obj, string property){
            try{
                ((System.Text.Json.JsonElement)obj).GetProperty(property);
                return true;
            }catch(Exception){ }
            return false;
        }
        public static int GetValuePropertyInt32(Object obj, string property){
            try{
                return ((System.Text.Json.JsonElement)obj).GetProperty(property).GetInt32();
            }catch(Exception){ }
            return 0;
        }

        public static string GetValuePropertyString(Object obj, string property){
            try{
                return ((System.Text.Json.JsonElement)obj).GetProperty(property).GetString();
            }catch(Exception){ }
            return "";
        }

        public static DateTime GetValuePropertyDateTime(Object obj, string property){
            try{
                return ((System.Text.Json.JsonElement)obj).GetProperty(property).GetDateTime();
            }catch(Exception){ }
            return DateTime.MinValue;
        }

    }
}
