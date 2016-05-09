using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace ALGAMES
{
 
 public static class TestUtils
 {
  
     //0->oponent wins,1,bot wins,2 not resolved,3 draw 
      public static Dictionary<int,string> TranslateTable=new Dictionary<int,string>{
             {0,"0"},
             {1,"1"}
          };
      public static string convertToString(this int[,] arr)
        {
            StringBuilder sb=new StringBuilder("\n"); 
            for(int i=0;i<arr.GetLength(0);i++)
            {
                for(int j=0;j<arr.GetLength(1);j++)
                {
                    var val=arr[i,j];
                    string strVal=val<0?" ":TranslateTable[val];
                     sb.Append(strVal);
                }
              sb.Append("\n");
            }
            sb.Append("");
            return(sb.ToString());
            
        }
 
 }   
    
}
