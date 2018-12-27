using System;

namespace CSharp.Script.Compile
{
    public class UniqueNameGenerator
    {
        public string Generate()
        {
            var guid = Guid.NewGuid().ToString("N");

            return "UNG" + guid;
        }
    }
}