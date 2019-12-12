using RocketSystem.DbLink;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Classes
{
    public class LoginUserPosition
    {
        private static DataAccessLayer db = new DataAccessLayer();

        public static ArrayList CalFirstBonus(string mem, int num, int stage)
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {

                ArrayList testlist = new ArrayList();
                var membernum = mem;
                var bcnumber = num;
                int count = 0;
                int c = 0;
                int max = 0;
                int levl = 0;
                int levels = 0;
                int colomns = 0;
                //**get  gatalist vise levels//**************
                var getdata1 = db.StageOnes.GroupBy(d => d.treeLevel).ToList();
                var getdata2 = db.StageTwoes.GroupBy(d => d.treeLevel).ToList();
                var getdata3 = db.StageThrees.GroupBy(d => d.treeLevel).ToList();
                var getdata4 = db.StageFours.GroupBy(d => d.treeLevel).ToList();
                var getdata5 = db.StageFives.GroupBy(d => d.treeLevel).ToList();

                var memberBClist = getBClist(membernum, bcnumber, stage);

                //**get StageWise levels columns and count****////
                if (stage == 1)
                {
                    count = db.StageOnes.GroupBy(d => d.treeLevel).Count();//get level count
                    levels = Convert.ToInt32(memberBClist[0].treeLevel);
                    colomns = Convert.ToInt32(memberBClist[0].treeColumn);
                }
                else if (stage == 2)
                {
                    count = db.StageTwoes.GroupBy(d => d.treeLevel).Count();//get level count
                    levels = Convert.ToInt32(memberBClist[0].treeLevel);
                    colomns = Convert.ToInt32(memberBClist[0].treeColumn);
                }
                else if (stage == 3)
                {
                    count = db.StageThrees.GroupBy(d => d.treeLevel).Count();//get level count
                    levels = Convert.ToInt32(memberBClist[0].treeLevel);
                    colomns = Convert.ToInt32(memberBClist[0].treeColumn);
                }
                else if (stage == 4)
                {
                    count = db.StageFours.GroupBy(d => d.treeLevel).Count();//get level count
                    levels = Convert.ToInt32(memberBClist[0].treeLevel);
                    colomns = Convert.ToInt32(memberBClist[0].treeColumn);
                }
                else if (stage == 5)
                {
                    count = db.StageFives.GroupBy(d => d.treeLevel).Count();//get level count
                    levels = Convert.ToInt32(memberBClist[0].treeLevel);
                    colomns = Convert.ToInt32(memberBClist[0].treeColumn);
                }

                int fmin = colomns;
                int min = 0;
                for (int xp = levels; xp < count; xp++)
                {
                    c++;
                    if (stage == 1)
                        levl = Convert.ToInt32(getdata1[xp].Key);
                    else if (stage == 2)
                        levl = Convert.ToInt32(getdata2[xp].Key);
                    else if (stage == 3)
                        levl = Convert.ToInt32(getdata3[xp].Key);
                    else if (stage == 4)
                        levl = Convert.ToInt32(getdata4[xp].Key);
                    else if (stage == 5)
                        levl = Convert.ToInt32(getdata5[xp].Key);

                    max = (int)Math.Pow(2, c - 1) * fmin;
                    min = (int)Math.Pow(2, c - 1) * (fmin - 1) + 1;
                    var testlist2 = LoginUserPosition.datalist(levl, max, min, stage);
                    for (int arrcount = 0; arrcount < testlist2.Count; arrcount++)
                    {
                        testlist.Add(testlist2[arrcount]);
                    }
                }
                return testlist;
            }
        }
        //*********************GET BC NUM LIST *******************
        public static dynamic getBClist(string mem, int bcnum, int stage)
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                if (stage == 1)
                {
                    var memberBClist = db.StageOnes.Where(d => d.membershipNo == mem && d.treeId == bcnum).ToList();// get member of 0 BC NUMBER
                    return memberBClist;
                }
                else if (stage == 2)
                {
                    var memberBClist = db.StageTwoes.Where(d => d.membershipNo == mem && d.treeId == bcnum).ToList();// get member of 0 BC NUMBER
                    return memberBClist;
                }
                else if (stage == 3)
                {
                    var memberBClist = db.StageThrees.Where(d => d.membershipNo == mem && d.treeId == bcnum).ToList();// get member of 0 BC NUMBER
                    return memberBClist;
                }
                else if (stage == 4)
                {
                    var memberBClist = db.StageFours.Where(d => d.membershipNo == mem && d.treeId == bcnum).ToList();// get member of 0 BC NUMBER
                    return memberBClist;
                }
                else if (stage == 5)
                {
                    var memberBClist = db.StageFives.Where(d => d.membershipNo == mem && d.treeId == bcnum).ToList();// get member of 0 BC NUMBER
                    return memberBClist;
                }
                return null;
            }
        }
        //***********GET DATA****************************** 
        public static dynamic datalist(int lev, long max, long rel, int stage)
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                if (stage == 1)
                {
                    var getldata = db.StageOnes.Where(d => d.treeLevel == lev && d.treeColumn >= rel && d.treeColumn <= max).ToList();//GET the number of positions filled in LEVEL
                    return getldata;
                }
                else if (stage == 2)
                {
                    var getldata = db.StageTwoes.Where(d => d.treeLevel == lev && d.treeColumn >= rel && d.treeColumn <= max).ToList();//GET the number of positions filled in LEVEL
                    return getldata;
                }
                else if (stage == 3)
                {
                    var getldata = db.StageThrees.Where(d => d.treeLevel == lev && d.treeColumn >= rel && d.treeColumn <= max).ToList();//GET the number of positions filled in LEVEL
                    return getldata;
                }
                else if (stage == 4)
                {
                    var getldata = db.StageFours.Where(d => d.treeLevel == lev && d.treeColumn >= rel && d.treeColumn <= max).ToList();//GET the number of positions filled in LEVEL
                    return getldata;
                }
                else if (stage == 5)
                {
                    var getldata = db.StageFives.Where(d => d.treeLevel == lev && d.treeColumn >= rel && d.treeColumn <= max).ToList();//GET the number of positions filled in LEVEL
                    return getldata;
                }
                return 0;
            }
        }
    }
}