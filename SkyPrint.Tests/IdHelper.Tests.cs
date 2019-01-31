using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SkyPrint.Helpers
{
    [TestFixture]
    public class IdHelperTest
    {
        [Test]
        public void Should_cut_ids_from_request_string()
        {
            var query = "28842-";
            var helper = new IdHelper();
            var result = helper.CutFirstTwoNumbers(query);
            Assert.AreEqual("28842", result);

            query = "28842";
            result = helper.CutFirstTwoNumbers(query);
            Assert.AreEqual("28842", result);

            query = "28843_2_SUSHI_R1_SKY_МАТОВЫЙ_225гр_ЛАМИНАТ_8_16_штук_дно_145_105_высота_40_мм_4_0_2 500шт__Палочки_для_суши_23_см_1 000шт";
            result = helper.CutFirstTwoNumbers(query);
            Assert.AreEqual("28843_2", result);

            query = "28843_2__2__SUSHI_R1_SKY_МАТОВЫЙ_225гр_ЛАМИНАТ_8_16_штук_дно_145_105_высота_40_мм_4_0_2 500шт__Палочки_для_суши_23_см_1 000шт";
            result = helper.CutFirstTwoNumbers(query);
            Assert.AreEqual("28843_2", result);
        }
    }
}
