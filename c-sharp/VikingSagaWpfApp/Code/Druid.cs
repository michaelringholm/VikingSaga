using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingSagaWpfApp.Code;

namespace VikingSaga.Code
{
    public class Druid : Hero
    {
        public override List<Level> GetLevels()
        {
            var levels = new List<Level>();
            levels.Add(new Level { LevelNo = 1, StartXP = 0, EndXP = 1000 });
            levels.Add(new Level { LevelNo = 2, StartXP = 1001, EndXP = 2000 });
            levels.Add(new Level { LevelNo = 3, StartXP = 2001, EndXP = 3000 });
            levels.Add(new Level { LevelNo = 4, StartXP = 3001, EndXP = 5000 });
            levels.Add(new Level { LevelNo = 5, StartXP = 5001, EndXP = 10000 });
            levels.Add(new Level { LevelNo = 6, StartXP = 10001, EndXP = 20000 });
            levels.Add(new Level { LevelNo = 7, StartXP = 20001, EndXP = 30000 });
            levels.Add(new Level { LevelNo = 8, StartXP = 30001, EndXP = 45000 });
            levels.Add(new Level { LevelNo = 9, StartXP = 45001, EndXP = 60000 });
            levels.Add(new Level { LevelNo = 10, StartXP = 60001, EndXP = 80000 });

            return levels;
        }
    }
}
