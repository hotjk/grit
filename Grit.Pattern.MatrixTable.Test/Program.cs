using Grit.MatrixTable.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Grit.Pattern.MatrixTable.Test
{
    class Program
    {
        public class BuilderDemo
        {
            public class Fee
            {
                public int Fee1 { get; set; }
                public int Fee2 { get; set; }

                public static Regex reg = new Regex("(\\d+),\\s*(\\d+)");
                public override string ToString()
                {
                    return string.Format(@"({0}, {1})", Fee1, Fee2);
                }

                public static Fee Parse(string script)
                {
                    var match = reg.Match(script);
                    return new Fee()
                    {
                        Fee1 = int.Parse(match.Groups[1].Value),
                        Fee2 = int.Parse(match.Groups[2].Value)
                    };
                }
            }

            public void Matrix1()
            {
                var matrix1 = MatrixBuilder.When().Int().String().Decimal().Then().Value(Fee.Parse).Parse(
    @"
--  Column1 | Column2       | Column3  | Value
-------------------------------------------------
    (0,10)  | 河北省,山%     | (, 10.5) | (1\,2)
    [10,20) | 河北省,山%     | (, 10.5) | (2\,3)
    [20,)   | 河北省,山%     | (, 10.5) | (3\,4)
    (0,10)  | 广东省         | (, 10.5) | (4\,5)
    [10,20) | 广东省         | (, 10.5) | (5\,6)
    [20,)   | 广东省         | (, 10.5) | (6\,7)
    (0,10)  | 河北省,山%     | [10.5,)  | (7\,8)
    [10,20) | 河北省,山%     | [10.5,)  | (8\,9)
    [20,)   | 河北省,山%     | [10.5,)  | (9\,10)
    (0,10)  | 广东省         | [10.5,)  | (10\,11)
    [10,20) | 广东省         | [10.5,)  | (11\,12)
    [20,)   | 广东省         | [10.5,)  | (12\,100)
");
                var age = 20;
                var province = "广东省";
                var size = new decimal(12);
                var result = matrix1.Calc(age, province, size);
                Console.WriteLine("{0} {1} {2} = {3}", age, province, size, result[0]);
            }

            public void Matrix2()
            {
                var result = MatrixBuilder.When().Int().String().Then().Int().Decimal().Parse(
@"
(,18],[60,) | 男 | 100, 10.1
(,18],[60,) | 女 | 120, 8.9
            | 男 | 160, 6.6
            | 女 | 180, 4.2
"
                    ).Calc(32, "男");
                Console.WriteLine("{0} {1}", result[0], result[1]);
            }
        }

        static void Main(string[] args)
        {
            BuilderDemo demo = new BuilderDemo();
            demo.Matrix1();
            demo.Matrix2();
        }
    }
}
