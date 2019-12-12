using RocketSystem.DbLink;
using RocketSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RocketSystem.Classes
{
    public class GenerateCsvFile
    {
        private static DataAccessLayer db = new DataAccessLayer();

        public static void generatingCsvFile()
        {

        }

        public static void generateCsvHeader(StreamWriter streamWriter) // making the header of the csv file
        {
            string date = DateTime.Now.Month.ToString().PadLeft(2, '0');// getting the current month for the csv header
            date = date + "25";
            streamWriter.Write("12100119064564ﾌｵ-ｴｽｴｽ                                 " + date + "9900ﾕｳﾁﾖ           019               20265866                 \r\n");//creating the header
        }

        public static double generateCsvBody(StreamWriter streamWriter, List<User> users, dynamic value)//creating the csv file body
        {
            //if it is post bank then get the branch code and acccount number from the substring of the account number
            if (users[0].bankNameKatakana.ToString() == "ﾕｳﾁﾖ")
            {
                users[0].transferDestinationBranchCode = users[0].transferAccountNumber.ToString().Substring(1, 3);
                users[0].transferAccountNumber = users[0].transferAccountNumber.ToString().Substring(6, 7);
            }
            double depositAmount = value.paidIntoduceBonus + value.paidPresentageBonus + value.paidThirdStageBonus + value.paidFifthStageBonus;

            streamWriter.Write("2" + users[0].transferDestinationBank + users[0].bankNameKatakana.PadRight(15, ' ') + users[0].transferDestinationBranchCode + "                   " + users[0].accountClassification + users[0].transferAccountNumber + users[0].katakanaName.PadRight(30, ' ') + depositAmount.ToString().PadLeft(10, '0') + "1" + value.memberId.ToString().PadLeft(6, '0') + value.bcNumber.ToString().PadLeft(4, '0') + "0000000000         \r\n");
            return depositAmount;
        }

        public static void generateCsvFooter(StreamWriter streamWriter, int noOfItems, string grandTotalDepositAmount)// creating the footer of the csv file
        {
            string totalNoOfItems = noOfItems.ToString().PadLeft(6, '0');
            grandTotalDepositAmount = grandTotalDepositAmount.ToString().PadLeft(12, '0');

            streamWriter.Write("8" + totalNoOfItems + grandTotalDepositAmount + "                                                                                                     \r\n");
            streamWriter.Write("9                                                                                                                       ");
        }
    }
}