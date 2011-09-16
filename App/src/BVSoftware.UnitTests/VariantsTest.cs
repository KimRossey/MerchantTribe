﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Catalog.Options;

namespace BVSoftware.UnitTests
{
    [TestClass]
    public class VariantsTest
    {
        private OptionList GetSampleOptions()
        {
            OptionList result = new OptionList();

            BVSoftware.Commerce.Catalog.Option option1 = Option.Factory(OptionTypes.DropDownList);
            option1.Name = "Color";
            option1.Bvin = "1";
            option1.IsShared = false;
            option1.IsVariant = true;
            option1.Items.Add(new OptionItem() { Bvin = "101", Name = "Red", OptionBvin = "1", SortOrder = 1, IsLabel = false });
            option1.Items.Add(new OptionItem() { Bvin = "102", Name = "Green", OptionBvin = "1", SortOrder = 2, IsLabel = false });
            result.Add(option1);

            BVSoftware.Commerce.Catalog.Option option2 = Option.Factory(OptionTypes.RadioButtonList);
            option2.Name = "Size";
            option2.Bvin = "2";
            option2.IsShared = false;
            option2.IsVariant = true;
            option2.Items.Add(new OptionItem() { Bvin = "201", Name = "S", OptionBvin = "2", SortOrder = 1, IsLabel = false });
            option2.Items.Add(new OptionItem() { Bvin = "202", Name = "M", OptionBvin = "2", SortOrder = 2, IsLabel = false });
            option2.Items.Add(new OptionItem() { Bvin = "203", Name = "L", OptionBvin = "2", SortOrder = 3, IsLabel = false });
            result.Add(option2);

            return result;
        }
        [TestMethod]
        public void CanGenerateCorrectSelectionData()
        {
            OptionList options = GetSampleOptions();

            VariantList target = new VariantList();
            BVApplication bvapp = BVApplication.InstantiateForMemory(new RequestContext());

            List<OptionSelectionList> data = bvapp.CatalogServices.VariantsGenerateAllPossibleSelections(options);

            Assert.IsNotNull(data, "Data should not be null");
            Assert.AreEqual(6, data.Count, "There should be six possible combinations");

            List<string> keys = new List<string>();
            foreach (OptionSelectionList o in data)
            {
                string k = OptionSelection.GenerateUniqueKeyForSelections(o);
                keys.Add(k);
            }
            Assert.AreEqual(6, keys.Count, "Key Count should be six.");
            Assert.IsTrue(keys.Contains("1-101|2-201|"), "Keys should contain 1-101|2-201|");
            Assert.IsTrue(keys.Contains("1-101|2-202|"), "Keys should contain 1-101|2-202|");
            Assert.IsTrue(keys.Contains("1-101|2-203|"), "Keys should contain 1-101|2-203|");
            Assert.IsTrue(keys.Contains("1-102|2-201|"), "Keys should contain 1-102|2-201|");
            Assert.IsTrue(keys.Contains("1-102|2-202|"), "Keys should contain 1-102|2-202|");
            Assert.IsTrue(keys.Contains("1-102|2-203|"), "Keys should contain 1-102|2-203|");
        }
    }
}
