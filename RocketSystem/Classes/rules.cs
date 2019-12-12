using RocketSystem.DbLink;
using RocketSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RocketSystem.Classes
{
    public class ReturnStage
    {
        public static long level { get; set; }
        public static long column { get; set; }
    }

    public class rules
    {
        private static DataAccessLayer db = new DataAccessLayer();
        static long lvl;
        static long clm;

        public static void submain(int xx, long yy, long stage)
        {
            // var getdata = db.trees.ToList();
            long count = 0;
            var getdatas = db.StageOnes.GroupBy(d => d.treeLevel).ToList();
            var getdata2 = db.StageTwoes.GroupBy(d => d.treeLevel).ToList();
            var getdata3 = db.StageThrees.GroupBy(d => d.treeLevel).ToList();
            var getdata4 = db.StageFours.GroupBy(d => d.treeLevel).ToList();
            var getdata5 = db.StageFives.GroupBy(d => d.treeLevel).ToList();
            if (stage == 1)
                count = db.StageOnes.GroupBy(d => d.treeLevel).Count();//get level count
            else if (stage == 2)
                count = db.StageTwoes.GroupBy(d => d.treeLevel).Count();//get level count 
            else if (stage == 3)
                count = db.StageThrees.GroupBy(d => d.treeLevel).Count();//get level count 
            else if (stage == 4)
                count = db.StageFours.GroupBy(d => d.treeLevel).Count();//get level count
            else if (stage == 5)
                count = db.StageFives.GroupBy(d => d.treeLevel).Count();//get level count
            long c = 0;
            long lev = 0;
            int xp = 1;
            long lcount = 0;
            long lside = 0, rside = 0;
            long rcount = 0;
            long posi = yy;
            int longlvl = ++xx;
            long max = 0;
            long min = posi;
            long pmin = 0;
            long ltx = 0;

            long fmin = posi;

            for (xp = longlvl; xp < count; xp++)
            {
                for (int x = xp; x < count; x++)// loop through level
                {
                    c++;
                    if (stage == 1)
                        lev = getdatas[x].Key;
                    else if (stage == 2)
                        lev = getdata2[x].Key;
                    else if (stage == 3)
                        lev = getdata3[x].Key;
                    else if (stage == 4)
                        lev = getdata4[x].Key;
                    else if (stage == 5)
                        lev = getdata5[x].Key;

                    max = (long)Math.Pow(2, x - xp + 1) * fmin;
                    min = (long)Math.Pow(2, x - xp + 1) * (fmin - 1) + 1;

                    ltx = (min + max - 1) / 2;
                    lside = rules.rule1m(lev, ltx, min, stage);
                    rside = rules.rule1m(lev, max, ltx + 1, stage);

                    lcount = lside + lcount;
                    rcount = rside + rcount;

                    if (rcount == lcount && lcount == 0)
                    {
                        break;

                    }
                    if (lcount != 0 && rcount == 0)
                    {
                        break;
                    }
                    pmin = max;

                }
                if (rcount == lcount && lcount == 0)
                {
                    lvl = lev;

                    clm = (fmin * 2) - 1;
                    break;

                }
                if (lcount != 0 && rcount == 0)
                {
                    lvl = lev;

                    clm = (fmin * 2);


                    break;
                }
                if (lcount == 1 && rcount == 1 && count == xp)
                {
                    lvl = ++xp;
                    clm = (posi * 2) - 1;
                    break;
                }

                if (lcount <= rcount)
                {
                    fmin = (fmin * 2) - 1;
                }
                else
                {
                    fmin = fmin * 2;
                }

                lcount = 0;
                rcount = 0;
            }
            if (count == longlvl)
            {
                lvl = longlvl;
                clm = (posi * 2) - 1;

            }
            if (count == xp)
            {
                lvl = xp;
                clm = (fmin * 2) - 1;

            }

        }

        public static void subFree(int xx, long yy, int stage)
        {
            int co = 0;
            string status = "";
            while (true)
            {
                if (status == "done")
                {
                    break;
                }
                xx++;
                co++;
                long min = (long)Math.Pow(2, co) * (yy - 1) + 1;//find min position of the range
                long max = (long)Math.Pow(2, co) * yy;//find max potition of the range
                long stone = rule1m(xx, max, min, stage);//find how many position this lavel have
                long maxlenth = (long)Math.Pow(2, co);//maximam position count of the range
                if (stone == maxlenth && stone != 0)
                {
                    continue;//this level postion and this level shoud me have maximum positions are equeal then skip this level
                }
                else if (stone == 0)
                {
                    ReturnStage.level = xx;
                    ReturnStage.column = (long)Math.Pow(2, (co)) * (yy - 1) + 1;
                    break;
                }
                else
                {
                    //find midel position number of this range
                    int xo = 0;
                    long mid = 0;
                    while (true)
                    {
                        mid = (min + max - 1) / 2;
                        xo++;
                        long lside = rule1m(xx, mid, min, stage); //find how mutch position in left side  of this range
                        long rside = rule1m(xx, max, (mid + 1), stage);//find how mutch position in right side  of this range
                        if (lside <= rside && rside != 0)
                        {
                            max = mid;

                        }
                        else if (lside == 0 && rside == 0)
                        {
                            ReturnStage.level = xx;
                            ReturnStage.column = min;
                            status = "done";
                            return;
                        }
                        else if (lside == 1 && rside == 0 && mid == lside)
                        {
                            ReturnStage.level = xx;
                            ReturnStage.column = max;
                            status = "done";
                            return;

                        }


                        else
                        {
                            min = (mid + 1);

                        }
                    }
                }

            }
        }
        public static long returnlevel()
        {
            return ReturnStage.level;
        }
        public static long returncolumn()
        {

            return ReturnStage.column;
        }
        public static long jumpingtree(long lev, long clm, long stage)
        {
            long min = 0, max = 0;
            long stvalue = clm / 4;
            if (clm % 4 != 0)
            {
                min = (stvalue * 4) + 1;
                max = (stvalue + 1) * 4;
            }
            else
            {
                max = clm;
                min = clm - 3;
            }
            long az = rule1m(lev, max, min, stage);
            long jpos = 0;
            if (az == 3)
            {
                jpos = max / (long)Math.Pow(2, 2);
            }
            return jpos;
        }
        public static void underJumping(long lev, long posi, long stage)
        {
            long count = 0;
            long max, min;

            while (true)
            {
                count++;
                try
                {
                    max = (long)Math.Pow(2, count) * posi;
                    min = (long)Math.Pow(2, count) * (posi - 1) + 1;
                    long tclm = underJumpingcount((lev + count), min, max, stage);
                    if (tclm != 0)
                    {
                        var getldata = underJumpingfresult((lev + count), min, max, stage);
                        string memno = getldata.membershipNo;
                        long bcno = getldata.bcNo;
                        var secondposition = underJumpingsresult(memno, bcno, stage);

                        ReturnStage.level = secondposition.level;
                        ReturnStage.column = secondposition.column;

                        return;

                    }
                    else
                    {
                        ReturnStage.level = 0;
                        ReturnStage.column = 1;
                        return;
                    }
                }
                catch (Exception)
                {

                }
            }
            //return;
        }
        public static long underJumpingcount(long lev, long min, long max, long stage)
        {
            long tclm = 0;
            if (stage == 1)
            {
                tclm = db.StageOnes.Where(d => d.treeLevel == lev && d.treeColumn >= min && d.treeColumn <= max).Count();

            }
            else if (stage == 2)
            {
                tclm = db.StageTwoes.Where(d => d.treeLevel == lev && d.treeColumn >= min && d.treeColumn <= max).Count();

            }

            else if (stage == 3)
            {
                tclm = db.StageThrees.Where(d => d.treeLevel == lev && d.treeColumn >= min && d.treeColumn <= max).Count();

            }
            else if (stage == 4)
            {
                tclm = db.StageFours.Where(d => d.treeLevel == lev && d.treeColumn >= min && d.treeColumn <= max).Count();

            }
            else if (stage == 5)
            {
                tclm = db.StageFives.Where(d => d.treeLevel == lev && d.treeColumn >= min && d.treeColumn <= max).Count();

            }
            return tclm;
        }
        public static dynamic underJumpingfresult(long lev, long min, long max, long stage)
        {
            if (stage == 1)
            {
                var getldata = db.StageOnes.Where(d => d.treeLevel == lev && d.treeColumn >= min && d.jump == 2 && d.treeColumn <= max).OrderBy(q => q.entryDate).First();
                return getldata;
            }
            else if (stage == 2)
            {
                var getldata = db.StageTwoes.Where(d => d.treeLevel == lev && d.treeColumn >= min && d.jump == 2 && d.treeColumn <= max).OrderBy(q => q.entryDate).First();
                return getldata;
            }
            else if (stage == 3)
            {
                var getldata = db.StageThrees.Where(d => d.treeLevel == lev && d.treeColumn >= min && d.jump == 2 && d.treeColumn <= max).OrderBy(q => q.entryDate).First();
                return getldata;
            }
            else if (stage == 4)
            {
                var getldata = db.StageFours.Where(d => d.treeLevel == lev && d.treeColumn >= min && d.jump == 2 && d.treeColumn <= max).OrderBy(q => q.entryDate).First();
                return getldata;
            }
            else if (stage == 5)
            {
                var getldata = db.StageFives.Where(d => d.treeLevel == lev && d.treeColumn >= min && d.jump == 2 && d.treeColumn <= max).OrderBy(q => q.entryDate).First();
                return getldata;
            }
            return null;
        }
        public static dynamic underJumpingsresult(string memno, long bcno, long stage)
        {

            if (stage == 1)
            {
                var secondposition = db.StageTwoes.Where(d => d.membershipNo == memno && d.bcNo == bcno).First();
                return secondposition;
            }
            else if (stage == 2)
            {
                var secondposition = db.StageThrees.Where(d => d.membershipNo == memno && d.bcNo == bcno).First();
                return secondposition;
            }
            else if (stage == 3)
            {
                var secondposition = db.StageFours.Where(d => d.membershipNo == memno && d.bcNo == bcno).First();
                return secondposition;
            }
            else if (stage == 4)
            {
                var secondposition = db.StageFives.Where(d => d.membershipNo == memno && d.bcNo == bcno).First();
                return secondposition;
            }


            return null;
        }
        public static long sendBcNo(string mem, long stage)
        {

            if (stage == 1)
            {
                long getldata = db.StageOnes.Where(d => d.membershipNo == mem).Count();
                return getldata;
            }
            else if (stage == 2)
            {
                long getldata = db.StageTwoes.Where(d => d.membershipNo == mem && d.jumpHistory == "2").Count();
                return getldata;
            }
            else if (stage == 3)
            {
                long getldata = db.StageThrees.Where(d => d.membershipNo == mem && d.jumpHistory == "3").Count();
                return getldata;
            }
            return 0;

        }
        public static StageOne introduceposi(string code)
        {
            var getdata = db.StageOnes.Where(d => d.positionCode == code).FirstOrDefault();
            if (getdata == null)
            {
                getdata = db.StageOnes.First();
                return getdata;
            }
            return getdata;
        }
        public static string sendCoundResult(long level, long column, string membershipNo, long bcNo, DateTime idate, long freestages, int position, long tempposition)
        {
            long count = 0;
            long round = 0;
            string sendcode = "";
            //add on 2019 / 07 / 3
            if (freestages == 0)
            {
                var getdata = db.TemporaryPositions.Where(d => d.positionId == tempposition).ToList();
                return getdata[position].positionCode;
            }

            else
            {
                while (true)
                {

                    if (round == 0)
                    {
                        sendcode = IntroduceCode.sendInCode(level + "" + column + "" + membershipNo);
                    }
                    else if (round == 1)
                    {
                        sendcode = IntroduceCode.sendInCode(level + "" + column + "" + membershipNo + "" + bcNo);
                    }
                    else if (round == 3)
                    {
                        sendcode = IntroduceCode.sendInCode(level + "" + column + "" + membershipNo + "" + bcNo + "" + idate);
                    }
                    else
                    {
                        sendcode = IntroduceCode.sendInCode(level + "" + column + "" + membershipNo + "" + bcNo + "" + idate + "" + CodeGen.sendCode());
                    }
                    count = db.StageOnes.Where(d => d.positionCode == sendcode).Count(); ;
                    if (count == 0)
                    {
                        break;
                    }
                    round++;
                }
            }

            return sendcode;
        }

        public static long rule1m(long lev, long max, long rel, long stage)



        {
            if (stage == 1)
            {
                long getldata = db.StageOnes.Where(d => d.treeLevel == lev && d.treeColumn >= rel && d.treeColumn <= max).Count();//check the number of positions filled in each side

                return getldata;
            }
            else if (stage == 2)
            {
                long getldata = db.StageTwoes.Where(d => d.treeLevel == lev && d.treeColumn >= rel && d.treeColumn <= max).Count();//check the number of positions filled in each side

                return getldata;
            }
            else if (stage == 3)
            {
                long getldata = db.StageThrees.Where(d => d.treeLevel == lev && d.treeColumn >= rel && d.treeColumn <= max).Count();//check the number of positions filled in each side

                return getldata;
            }
            else if (stage == 4)
            {
                long getldata = db.StageFours.Where(d => d.treeLevel == lev && d.treeColumn >= rel && d.treeColumn <= max).Count();//check the number of positions filled in each side

                return getldata;
            }
            else if (stage == 5)
            {
                long getldata = db.StageFives.Where(d => d.treeLevel == lev && d.treeColumn >= rel && d.treeColumn <= max).Count();//check the number of positions filled in each side

                return getldata;
            }

            return 0;
        }

        public static dynamic findTreeId(long level, long jpos, long stage)
        {
            if (stage == 1)
            {
                var getdatas = db.StageOnes.Where(d => d.treeLevel == level && d.treeColumn == jpos).First();
                return getdatas;
            }
            else if (stage == 2)
            {
                var getdatas = db.StageTwoes.Where(d => d.treeLevel == level && d.treeColumn == jpos).First();
                return getdatas;
            }
            else if (stage == 3)
            {
                var getdatas = db.StageThrees.Where(d => d.treeLevel == level && d.treeColumn == jpos).First();
                return getdatas;
            }
            else if (stage == 4)
            {
                var getdatas = db.StageFours.Where(d => d.treeLevel == level && d.treeColumn == jpos).First();
                return getdatas;
            }
            else if (stage == 5)
            {
                var getdatas = db.StageFives.Where(d => d.treeLevel == level && d.treeColumn == jpos).First();
                return getdatas;
            }

            return null;
        }
        public static dynamic findTreeId2(long bcno, string memno, long stage)
        {
            if (stage == 1)
            {
                var getdatas = db.StageOnes.Where(d => d.membershipNo == memno).FirstOrDefault();
                return getdatas;
            }
            else if (stage == 2)
            {
                var getdatas = db.StageTwoes.Where(d => d.membershipNo == memno).FirstOrDefault();
                if (getdatas == null)
                {
                    var getdata2 = db.StageTwoes.First();
                    return getdata2;
                }
                return getdatas;
            }
            else if (stage == 3)
            {
                var getdatas = db.StageThrees.Where(d => d.membershipNo == memno).FirstOrDefault();
                if (getdatas == null)
                {
                    var getdata2 = db.StageThrees.First();
                    return getdata2;
                }
                return getdatas;
            }
            else if (stage == 4)
            {
                var getdatas = db.StageFours.Where(d => d.membershipNo == memno).FirstOrDefault();
                if (getdatas == null)
                {
                    var getdata2 = db.StageFours.First();
                    return getdata2;
                }
                return getdatas;
            }
            else if (stage == 5)
            {
                var getdatas = db.StageFives.Where(d => d.membershipNo == memno).FirstOrDefault();
                if (getdatas == null)
                {
                    var getdata2 = db.StageFives.First();
                    return getdata2;
                }
                return getdatas;
            }
            return null;
        }

        public static void HavenextTable(long lvl, long clm, long stage)
        {
            long level1 = lvl;
            long column1 = clm;
            long stage1 = stage;
            long Level = lvl;
            long Column = clm;
            long b = 1;
            long parent = 0;
            long treeids = 0;
            long loopid = 0;

            var bcs = selectmybcs(stage, lvl, clm);
            if (Level == 0)
            {
                underJumping(Level, Column, stage);
            }
            else if (bcs != null)
            {
                ReturnStage.level = bcs.treeLevel;
                ReturnStage.column = bcs.treeColumn;
            }

            else if (Level != 0 || bcs == null)
            {

                for (long i = Level - 1; i >= 1; i--)
                {
                    if (Column % 2 == 1)
                    {
                        parent = (Column + 1) / 2;
                        var jumpinglist = sendDataTree(parent, i, stage);

                        treeids = -1;
                        if (jumpinglist != null)
                        {
                            treeids = jumpinglist.treeId;
                        }

                        var trees2 = db.StageTwoes.Where(d => d.previousPosition == treeids).FirstOrDefault();
                        var trees3 = db.StageThrees.Where(d => d.previousPosition == treeids).FirstOrDefault();
                        var trees4 = db.StageFours.Where(d => d.previousPosition == treeids).FirstOrDefault();
                        var trees5 = db.StageFives.Where(d => d.previousPosition == treeids).FirstOrDefault();
                        if (stage == 1 && trees2 != null)
                        {
                            ReturnStage.level = trees2.treeLevel;
                            ReturnStage.column = trees2.treeColumn;
                            loopid = 4;
                            break;
                        }
                        else if (stage == 2 && trees3 != null)
                        {
                            ReturnStage.level = trees3.treeLevel;
                            ReturnStage.column = trees3.treeColumn;
                            loopid = 4;
                            break;
                        }
                        if (stage == 3 && trees4 != null)
                        {
                            ReturnStage.level = trees4.treeLevel;
                            ReturnStage.column = trees4.treeColumn;
                            loopid = 4;
                            break;
                        }
                        if (stage == 4 && trees5 != null)
                        {
                            ReturnStage.level = trees5.treeLevel;
                            ReturnStage.column = trees5.treeColumn;
                            loopid = 4;
                            break;
                        }
                    }

                    else if (Column % 2 == 0)
                    {
                        parent = Column / 2;
                        var jumpinglist = sendDataTree(parent, i, stage);

                        treeids = 0;
                        if (jumpinglist != null)
                        {
                            treeids = jumpinglist.treeId;
                        }
                        var trees2 = db.StageTwoes.Where(d => d.previousPosition == treeids).FirstOrDefault();
                        var trees3 = db.StageThrees.Where(d => d.previousPosition == treeids).FirstOrDefault();
                        var trees4 = db.StageFours.Where(d => d.previousPosition == treeids).FirstOrDefault();
                        var trees5 = db.StageFives.Where(d => d.previousPosition == treeids).FirstOrDefault();

                        if (stage == 1 && trees2 != null)
                        {
                            ReturnStage.level = trees2.treeLevel;
                            ReturnStage.column = trees2.treeColumn;
                            loopid = 4;
                            break;
                        }
                        else if (stage == 2 && trees3 != null)
                        {
                            ReturnStage.level = trees3.treeLevel;
                            ReturnStage.column = trees3.treeColumn;
                            loopid = 4;
                            break;
                        }
                        if (stage == 3 && trees4 != null)
                        {
                            ReturnStage.level = trees4.treeLevel;
                            ReturnStage.column = trees4.treeColumn;
                            loopid = 4;
                            break;
                        }
                        if (stage == 4 && trees5 != null)
                        {
                            ReturnStage.level = trees5.treeLevel;
                            ReturnStage.column = trees5.treeColumn;
                            loopid = 4;
                            break;
                        }


                    }




                    Level = Level - 1;
                    Column = parent;



                }
                if (loopid == 0)
                {
                    underJumping(level1, column1, stage1);

                }


            }
        }
        public static dynamic selectmybcs(long stage, long level, long column)
        {

            if (stage == 1)
            {
                var newlist = db.StageOnes.Where(d => d.treeLevel == level && d.treeColumn == column).FirstOrDefault();
                var newlist1 = db.StageTwoes.Where(d => d.membershipNo == newlist.membershipNo).FirstOrDefault();
                return newlist1;
            }
            else if (stage == 2)
            {
                var newlist = db.StageTwoes.Where(d => d.treeLevel == level && d.treeColumn == column).FirstOrDefault();
                var newlist1 = db.StageThrees.Where(d => d.membershipNo == newlist.membershipNo).FirstOrDefault();
                return newlist1;
            }
            else if (stage == 3)
            {
                var newlist = db.StageThrees.Where(d => d.treeLevel == level && d.treeColumn == column).FirstOrDefault();
                var newlist1 = db.StageFours.Where(d => d.membershipNo == newlist.membershipNo).FirstOrDefault();
                return newlist1;
            }
            else if (stage == 4)
            {
                var newlist = db.StageFours.Where(d => d.treeLevel == level && d.treeColumn == column).FirstOrDefault();
                var newlist1 = db.StageFives.Where(d => d.membershipNo == newlist.membershipNo).FirstOrDefault();
                return newlist1;
            }
            return null;
        }
        public static int findPackage(long column, int level, int stage)
        {
            if (stage == 1)
            {
                var getdata = db.StageOnes.Where(a => a.treeLevel == level && a.treeColumn == column).First();
                return getdata.package;
            }
            else if (stage == 2)
            {
                var getdata = db.StageTwoes.Where(a => a.treeLevel == level && a.treeColumn == column).First();
                return getdata.package;
            }
            else if (stage == 3)
            {
                var getdata = db.StageThrees.Where(a => a.treeLevel == level && a.treeColumn == column).First();
                return getdata.package;
            }
            else if (stage == 4)
            {
                var getdata = db.StageFours.Where(a => a.treeLevel == level && a.treeColumn == column).First();
                return getdata.package;
            }
            return 0;
        }

        public static dynamic sendDataTree(long parent, long level, long stage)
        {
            if (stage == 1)
            {
                var datas = db.StageOnes.Where(d => d.treeColumn == parent && d.treeLevel == level && d.jump == 2).FirstOrDefault();

                return datas;
            }
            else if (stage == 2)
            {
                var datas = db.StageTwoes.Where(d => d.treeColumn == parent && d.treeLevel == level && d.jump == 3).FirstOrDefault();
                return datas;
            }
            else if (stage == 3)
            {
                var datas = db.StageThrees.Where(d => d.treeColumn == parent && d.treeLevel == level && d.jump == 4).FirstOrDefault();
                return datas;
            }
            else if (stage == 4)
            {
                var datas = db.StageFours.Where(d => d.treeColumn == parent && d.treeLevel == level && d.jump == 5).FirstOrDefault();
                return datas;
            }
            else if (stage == 5)
            {
                var datas = db.StageFives.Where(d => d.treeColumn == parent && d.treeLevel == level && d.jump == 2).FirstOrDefault();
                return datas;
            }

            return null;
        }
        public static Package packdata()
        {

            long getmax = db.Packages.Where(dt => dt.packageStartDate <= DateTime.Today && dt.packageEndDate >= DateTime.Today).Max(d => d.packagePriority);
            var getdatas = db.Packages.Where(d => d.packagePriority == getmax).First();
            // long result = DateTime.Compare(getdata[0].date, DateTime.Now);

            if (packageCount(getdatas.packageId) > (getdatas.noOfPosition - 1))
            {
                Package pk = new Package();
                pk = db.Packages.Find(getdatas.packageId);
                pk.packagePriority = 0;
                pk.status = 0;
                db.Packages.Attach(pk);
                db.Entry(pk).State = EntityState.Modified;
                db.SaveChanges();
                // rules.packdata();
            }
            else
            {
                return getdatas;
            }
            return null;
        }
        public static long packageCount(long packid)
        {
            long count = db.StageOnes.Where(d => d.package == packid && d.freeStatus == 0).Count();
            return count;
        }
        public static string sendPromoCode(long stage, string mem)
        {
            if (stage == 1)
            {
                var sendpromo = db.StageOnes.Where(d => d.membershipNo == mem).First();
                return sendpromo.positionCode;
            }

            return null;
        }
        public static dynamic sendTreedataFromid(long stage, long treeid)
        {
            if (stage == 2)
            {
                var sendatas = db.StageTwoes.Where(d => d.treeId == treeid).First();
                return sendatas;
            }
            else if (stage == 3)
            {
                var sendatas = db.StageThrees.Where(d => d.treeId == treeid).First();
                return sendatas;
            }
            return null;
        }

        public static Boolean paidvalidPositions(long temppositionsno)
        {
            //check paid position count is avalable
            try
            {
                var getdata = db.PositionDetails.Where(d => d.positionId == temppositionsno && d.systemUpdate == 1).First();
                long packages = Convert.ToInt32(getdata.package);
                var getpackage = db.Packages.Where(a => a.packageId == packages).First();
                long packagescount = db.StageOnes.Where(d => d.package == getpackage.packageId).Count();
                if (getpackage.noOfPosition <= packagescount)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            { return true; }
        }

        public static Boolean checkPositions(long temppositionsno)
        {
            //checked tempory position collection is update the system 
            long datacount = db.PositionDetails.Where(d => d.positionId == temppositionsno && d.systemUpdate == 1).Count();
            if (datacount == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // create levell 3 and level 5 free nodes
        public static long freePosition(long position, long level, long stage)
        {
            long min = 0, max = 0;
            if (position == 16)
            {
            }
            long stvalue = position / 8; //get for create the range
            if (position % 8 != 0)
            {
                min = (stvalue * 8) + 1; //find min value 
                max = (stvalue + 1) * 8; //find max value of the range
            }
            else
            {
                max = position;
                min = position - 7;
            }
            long az = rule1m(level, max, min, stage);
            long jpos = 0;
            if (az == 8)
            {
                jpos = max / (long)Math.Pow(2, 3);
            }
            return jpos;

        }
        //send promocode from tree position
        public static string sendCodeFromPosi(long level, long column, long stage)
        {
            if (stage == 3)
            {
                var getdata = db.StageThrees.Where(d => d.treeColumn == column && d.treeLevel == level).First();
                var datas = db.StageOnes.Where(a => a.membershipNo == getdata.membershipNo).First();
                return datas.positionCode;
            }
            else if (stage == 5)
            {
                var getdata = db.StageFives.Where(d => d.treeColumn == column && d.treeLevel == level).First();
                var datas = db.StageOnes.Where(a => a.membershipNo == getdata.membershipNo).First();
                return datas.positionCode;
            }
            return "";

        }
        //Free position tree id
        public static int sendResonId(long level, long column, long stage)
        {
            if (stage == 3)
            {
                var getdata = db.StageThrees.Where(d => d.treeColumn == column && d.treeLevel == level).First();

                return getdata.treeId;
            }
            else if (stage == 5)
            {
                var getdata = db.StageFives.Where(d => d.treeColumn == column && d.treeLevel == level).First();
                return getdata.treeId;
            }
            return 0;

        }

        public static dynamic getTempData(long tempposicollectionId)
        {
            var sendData = db.PositionDetails.Where(d => d.positionId == tempposicollectionId).First();
            return sendData;
        }
        public static string getTempDataintu(long tempposicollectionId)
        {
            var sendData = db.PositionDetails.Where(d => d.positionId == tempposicollectionId).First();
            var senddata3 = db.TemporaryPositions.Where(d => d.positionCode == sendData.introducePromoCode).FirstOrDefault();
            var getdata = db.PositionDetails.Where(d => d.positionId == senddata3.positionId).FirstOrDefault();
            var senddata4 = db.StageOnes.Where(d => d.membershipNo == getdata.membershipNo).FirstOrDefault();
            var senddata2 = db.StageOnes.Where(a => a.membershipNo == sendData.membershipNo).ToList();
            if (senddata2.Count > 0)
            {
                string getpromo = sendPromoCode(1, senddata2[0].membershipNo);
                return getpromo;
            }
            else if (senddata4 != null)
            {
                return senddata4.positionCode;
            }
            else
            {
                return sendData.introducePromoCode;
            }
        }
        public static long sendMemberCountInTree(string memberno)
        {
            long senddata = db.StageOnes.Where(d => d.membershipNo == memberno).Count();
            return senddata;
        }

        public static Boolean checkFreePosition(string memberNo, int bcno, int packstyle, int wherestage, string jumphis)
        {
            int stagebonus = 0;
            int wantbonusvalue = 0;
            Boolean freepositionaccsept = true;
            BonusCalculation ab = new BonusCalculation();
            switch (wherestage)
            {
                case 3:
                    StageThree tree3th = new StageThree();
                    tree3th.membershipNo = memberNo;
                    tree3th.bcNo = bcno;
                    tree3th.jumpHistory = jumphis;
                    StageThree tree2 = ab.CalculateBonusStageThree(tree3th);
                    wantbonusvalue = 320000;
                    stagebonus = tree2.bonus3;
                    switch (packstyle)
                    {
                        case 1:
                            switch (stagebonus)
                            {
                                case 320000:
                                    //320,000‬
                                    freepositionaccsept = true;
                                    break;

                                default:
                                    //320,000‬
                                    freepositionaccsept = false;
                                    break;
                            }
                            break;

                        case 2:

                            switch (stagebonus)
                            {
                                case 320000:
                                    //320,000‬
                                    freepositionaccsept = true;
                                    break;

                                default:
                                    //320,000‬
                                    freepositionaccsept = false;
                                    break;
                            }
                            break;

                        case 3:

                            switch (stagebonus)
                            {
                                case 32000:
                                    //320,000‬
                                    freepositionaccsept = true;
                                    break;

                                default:
                                    freepositionaccsept = false;
                                    break;
                            }
                            break;
                    }
                    break;

                case 5:

                    StageFive tree5th = new StageFive();
                    tree5th.membershipNo = memberNo;
                    tree5th.bcNo = bcno;
                    tree5th.jumpHistory = jumphis;
                    StageFive tree3 = ab.CalculateBonusStageFive(tree5th);
                    stagebonus = tree3.bonus5;

                    switch (packstyle)
                    {
                        case 1:
                            switch (stagebonus)
                            {
                                case 320000:

                                    freepositionaccsept = true;
                                    break;

                                default:

                                    freepositionaccsept = false;
                                    break;
                            }
                            break;

                        case 2:

                            switch (stagebonus)
                            {
                                case 320000:

                                    freepositionaccsept = true;
                                    break;

                                default:

                                    freepositionaccsept = false;
                                    break;
                            }
                            break;

                        case 3:

                            switch (stagebonus)
                            {
                                case 32000:

                                    freepositionaccsept = true;
                                    break;

                                default:
                                    freepositionaccsept = false;
                                    break;
                            }
                            break;
                    }
                    break;
            }
            return freepositionaccsept;
        }
    }
}