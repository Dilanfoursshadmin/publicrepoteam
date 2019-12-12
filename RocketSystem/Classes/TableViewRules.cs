using RocketSystem.DbLink;
using RocketSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Classes
{
    public class TableViewRules
    {
        private static DataAccessLayer db = new DataAccessLayer();

        public static dynamic sendTableResult(int stage, int bcno, string memno)//get data to create a all list table 
        {
            if (stage == 1)
            {
                var getdata = db.StageOnes.Where(d => d.membershipNo == memno && d.bcNo == (bcno+1)).First();
                return getdata;
            }
            else if (stage == 2)
            {
                var getdata = db.StageTwoes.Where(d => d.membershipNo == memno && d.bcNo == (bcno + 1)).First();
                return getdata;
            }
            else if (stage == 3)
            {
                var getdata = db.StageThrees.Where(d => d.membershipNo == memno && d.bcNo == (bcno + 1)).First();
                return getdata;
            }
            else if (stage == 4)
            {
                var getdata = db.StageFours.Where(d => d.membershipNo == memno && d.bcNo == (bcno + 1)).First();
                return getdata;
            }
            else if (stage == 5)
            {
                var getdata = db.StageFives.Where(d => d.membershipNo == memno && d.bcNo == (bcno + 1)).First();
                return getdata;
            }
            return null;
        }
        public static List<StageOne> sendDataBundleOne(int stage, string memno)
        {
            var treelist = new List<StageOne>();

            var skipbc = new List<int>();

            int bccount = db.StageOnes.Where(d => d.membershipNo == memno).Count();
            for (int co = 0; co < bccount; co++)
            {
                string brklp = "";
                for (int bcc = 0; bcc < skipbc.Count(); bcc++)
                {
                    if (skipbc[bcc] == co)
                        brklp = "yes";
                }

                if (brklp == "yes")
                {
                    brklp = "";
                    continue;
                }
                try
                {
                    int cout = 0;
                    StageOne tree = new StageOne();
                    tree = sendTableResult(stage, co, memno);
                    long fmin = tree.treeColumn;
                    int lev = tree.treeLevel;
                    int maxlev = db.StageOnes.Max(d => d.treeLevel);
                    for (int x = lev; x <= maxlev; x++)
                    {

                        try
                        {
                            long max = (long)Math.Pow(2, cout) * fmin;
                            long min = (long)Math.Pow(2, cout) * (fmin - 1) + 1;
                            var datas = db.StageOnes.Where(d => d.treeLevel == x && d.treeColumn >= min && d.treeColumn <= max).ToList();
                            int rmv = datas.Where(d => d.membershipNo == memno).Count();
                            if (rmv != 0)
                            {
                                var remove = datas.Where(d => d.membershipNo == memno).ToList();

                                for (int xx = 0; xx < rmv; xx++)
                                {

                                    skipbc.Add(remove[xx].bcNo);
                                }
                            }

                            for (int xt = 0; xt < datas.Count(); xt++)
                            {
                                treelist.Add(datas[xt]);
                            }
                            cout++;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            return treelist;
        }

        public static dynamic sendDataBundleTwo(int stage, string memno)
        {
            var treelist = new List<StageTwo>();

            var skipbc = new List<int>();

            int bccount = db.StageTwoes.Where(d => d.membershipNo == memno).Count();
            for (int co = 0; co < bccount; co++)
            {
                string brklp = "";
                for (int bcc = 0; bcc < skipbc.Count(); bcc++)
                {
                    if (skipbc[bcc] == co)
                        brklp = "yes";
                }

                if (brklp == "yes")
                {
                    brklp = "";
                    continue;
                }
                try
                {
                    int cout = 0;
                    StageTwo tree = new StageTwo();
                    tree = sendTableResult(stage, (co+1), memno);
                    long fmin = tree.treeColumn;
                    int lev = tree.treeLevel;
                    int maxlev = db.StageOnes.Max(d => d.treeLevel);
                    for (int x = lev; x <= maxlev; x++)
                    {
                        try
                        {
                            long max = (long)Math.Pow(2, cout) * fmin;
                            long min = (long)Math.Pow(2, cout) * (fmin - 1) + 1;
                            var datas = db.StageTwoes.Where(d => d.treeLevel == x && d.treeColumn >= min && d.treeColumn <= max).ToList();
                            int rmv = datas.Where(d => d.membershipNo == memno).Count();
                            if (rmv != 0)
                            {
                                var remove = datas.Where(d => d.membershipNo == memno).ToList();

                                for (int xx = 0; xx < rmv; xx++)
                                {

                                    skipbc.Add(remove[xx].bcNo);
                                }
                            }

                            for (int xt = 0; xt < datas.Count(); xt++)
                            {
                                treelist.Add(datas[xt]);
                            }
                            cout++;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }

                }
                catch (Exception)
                {
                    break;
                }
            }
            return treelist;
        }

        public static dynamic sendDataBundleThree(int stage, string memno)
        {
            var treelist = new List<StageThree>();

            var skipbc = new List<int>();

            int bccount = db.StageThrees.Where(d => d.membershipNo == memno).Count();
            for (int co = 0; co < bccount; co++)
            {
                string brklp = "";
                for (int bcc = 0; bcc < skipbc.Count(); bcc++)
                {

                    if (skipbc[bcc] == co)
                        brklp = "yes";
                }

                if (brklp == "yes")
                {
                    brklp = "";
                    continue;

                }
                try
                {
                    int cout = 0;
                    StageThree tree = new StageThree();
                    tree = sendTableResult(stage, co, memno);
                    long fmin = tree.treeColumn;
                    int lev = tree.treeLevel;
                    int maxlev = db.StageThrees.Max(d => d.treeLevel);
                    for (int x = lev; x <= maxlev; x++)
                    {

                        try
                        {
                            long max = (long)Math.Pow(2, cout) * fmin;
                            long min = (long)Math.Pow(2, cout) * (fmin - 1) + 1;
                            var datas = db.StageThrees.Where(d => d.treeLevel == x && d.treeColumn >= min && d.treeColumn <= max).ToList();
                            int rmv = datas.Where(d => d.membershipNo == memno).Count();
                            if (rmv != 0)
                            {
                                var remove = datas.Where(d => d.membershipNo == memno).ToList();

                                for (int xx = 0; xx < rmv; xx++)
                                {

                                    skipbc.Add(remove[xx].bcNo);
                                }
                            }

                            for (int xt = 0; xt < datas.Count(); xt++)
                            {
                                treelist.Add(datas[xt]);
                            }
                            cout++;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }

                }
                catch (Exception)
                {
                    break;
                }
            }
            return treelist;
        }

        public static dynamic sendDataBundleFour(int stage, string memno)
        {
            var treelist = new List<StageFour>();

            var skipbc = new List<int>();

            int bccount = db.StageFours.Where(d => d.membershipNo == memno).Count();
            for (int co = 0; co < bccount; co++)
            {
                string brklp = "";
                for (int bcc = 0; bcc < skipbc.Count(); bcc++)
                {

                    if (skipbc[bcc] == co)
                        brklp = "yes";
                }

                if (brklp == "yes")
                {
                    brklp = "";
                    continue;

                }
                try
                {
                    int cout = 0;
                    StageFour tree = new StageFour();
                    tree = sendTableResult(stage, co, memno);
                    long fmin = tree.treeColumn;
                    int lev = tree.treeLevel;
                    int maxlev = db.StageFours.Max(d => d.treeLevel);
                    for (int x = lev; x <= maxlev; x++)
                    {

                        try
                        {
                            long max = (long)Math.Pow(2, cout) * fmin;
                            long min = (long)Math.Pow(2, cout) * (fmin - 1) + 1;
                            var datas = db.StageFours.Where(d => d.treeLevel == x && d.treeColumn >= min && d.treeColumn <= max).ToList();
                            int rmv = datas.Where(d => d.membershipNo == memno).Count();
                            if (rmv != 0)
                            {
                                var remove = datas.Where(d => d.membershipNo == memno).ToList();

                                for (int xx = 0; xx < rmv; xx++)
                                {

                                    skipbc.Add(remove[xx].bcNo);
                                }
                            }

                            for (int xt = 0; xt < datas.Count(); xt++)
                            {
                                treelist.Add(datas[xt]);
                            }
                            cout++;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }

                }
                catch (Exception)
                {
                    break;
                }
            }
            return treelist;
        }
        public static dynamic sendDataBundleFive(int stage, string memno)
        {
            var treelist = new List<StageFive>();

            var skipbc = new List<int>();

            int bccount = db.StageFives.Where(d => d.membershipNo == memno).Count();
            for (int co = 0; co < bccount; co++)
            {
                string brklp = "";
                for (int bcc = 0; bcc < skipbc.Count(); bcc++)
                {

                    if (skipbc[bcc] == co)
                        brklp = "yes";
                }

                if (brklp == "yes")
                {
                    brklp = "";
                    continue;

                }
                try
                {
                    int cout = 0;
                    StageFive tree = new StageFive();
                    tree = sendTableResult(stage, co, memno);
                    long fmin = tree.treeColumn;
                    int lev = tree.treeLevel;
                    int maxlev = db.StageFives.Max(d => d.treeLevel);
                    for (int x = lev; x <= maxlev; x++)
                    {

                        try
                        {
                            long max = (long)Math.Pow(2, cout) * fmin;
                            long min = (long)Math.Pow(2, cout) * (fmin - 1) + 1;
                            var datas = db.StageFives.Where(d => d.treeLevel == x && d.treeColumn >= min && d.treeColumn <= max).ToList();
                            int rmv = datas.Where(d => d.membershipNo == memno).Count();
                            if (rmv != 0)
                            {
                                var remove = datas.Where(d => d.membershipNo == memno).ToList();

                                for (int xx = 0; xx < rmv; xx++)
                                {

                                    skipbc.Add(remove[xx].bcNo);
                                }
                            }

                            for (int xt = 0; xt < datas.Count(); xt++)
                            {
                                treelist.Add(datas[xt]);
                            }
                            cout++;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }

                }
                catch (Exception)
                {
                    break;
                }
            }
            return treelist;
        }
    }
}