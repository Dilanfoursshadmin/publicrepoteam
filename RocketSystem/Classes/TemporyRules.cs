using RocketSystem.DbLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Classes
{
    public class TemporyRules
    {
        private static DataAccessLayer db = new DataAccessLayer();

        public static string sendTempCode(string membershipNo, DateTime idate)
        {
            int count = 0;
            int count2 = 0;
            int round = 0;
            string sendcode = "";
            while (true)
            {

                if (round == 0)
                {
                    sendcode = IntroduceCode.sendInCode(membershipNo + "" + idate);
                }
                else if (round == 1)
                {
                    sendcode = IntroduceCode.sendInCode(idate + "" + membershipNo);
                }

                else
                {
                    sendcode = IntroduceCode.sendInCode(idate + "" + membershipNo + "" + CodeGen.sendCode());

                }
                count = db.StageOnes.Where(d => d.positionCode == sendcode).Count();
                count2 = db.TemporaryPositions.Where(d => d.positionCode == sendcode).Count();
                if (count == 0 && count2 == 0)
                {
                    break;
                }
                round++;
            }

            return sendcode;
        }
    }
}