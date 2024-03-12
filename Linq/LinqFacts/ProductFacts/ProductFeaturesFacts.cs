using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LinqProduct
{
    public class ProductFeaturesFacts
    {
        [Fact]
        public void Product_ProductsThatHasAtLeastOneFeature()
        {
            var oneCode = new Feature { Id = 1102 };
            var treeCode = new Feature { Id = 3648 };
            var fourCode = new Feature { Id = 401 };
            var fiveCode = new Feature { Id = 5987 };
            var sixCode = new Feature { Id = 6634 };
            var nineCode = new Feature { Id = 9124 };
            var sixeCode = new Feature { Id = 6801 };
            
            var features = new List<Feature>()
            {
                treeCode, fourCode, fiveCode, sixCode
            };

            var minge = new Product { Name = "minge", Features = new List<Feature>() { oneCode, treeCode } };
            var papusa = new Product { Name = "papusa", Features = new List<Feature>() { treeCode } };
            var bile = new Product { Name = "bile", Features = new List<Feature>() { sixeCode, oneCode} };
            var urs = new Product { Name = "urs", Features = new List<Feature>() { fiveCode, nineCode } };
            
            var products = new List<Product>()
            {
                minge, papusa, bile, urs
            };

            var result = ProductFeatures.AtLeastOneFeature(products, features);
            var expectedResult = new List<Product>()
            {
                minge, papusa, urs
            };

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Product_ProductsThatHasAllFeatures()
        {
            var oneCode = new Feature { Id = 1102 };
            var treeCode = new Feature { Id = 3648 };
            var fourCode = new Feature { Id = 401 };
            var fiveCode = new Feature { Id = 5987 };
            var sixCode = new Feature { Id = 6634 };
            var nineCode = new Feature { Id = 9124 };
            var sixeCode = new Feature { Id = 6801 };

            var features = new List<Feature>()
            {
                treeCode, fourCode, fiveCode, sixCode
            };

            var minge = new Product { Name = "minge", Features = new List<Feature>() { fiveCode, treeCode, fourCode, sixCode } };
            var papusa = new Product { Name = "papusa", Features = new List<Feature>() { treeCode } };
            var bile = new Product { Name = "bile", Features = new List<Feature>() { sixeCode, oneCode } };
            var urs = new Product { Name = "urs", Features = new List<Feature>() { fiveCode, nineCode } };

            var products = new List<Product>()
            {
                minge, papusa, bile, urs
            };

            var result = ProductFeatures.AllFeatures(products, features);
            var expectedResult = new List<Product>()
            {
                minge
            };

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Product_NotEvenOneFeature()
        {
            var oneCode = new Feature { Id = 1102 };
            var treeCode = new Feature { Id = 3648 };
            var fourCode = new Feature { Id = 401 };
            var fiveCode = new Feature { Id = 5987 };
            var sixCode = new Feature { Id = 6634 };
            var nineCode = new Feature { Id = 9124 };
            var sixeCode = new Feature { Id = 6801 };

            var features = new List<Feature>()
            {
                treeCode, fourCode, fiveCode, sixCode
            };

            var minge = new Product { Name = "minge", Features = new List<Feature>() { fiveCode, treeCode, fourCode, sixCode } };
            var papusa = new Product { Name = "papusa", Features = new List<Feature>() { treeCode } };
            var bile = new Product { Name = "bile", Features = new List<Feature>() { sixeCode, oneCode } };
            var urs = new Product { Name = "urs", Features = new List<Feature>() { fiveCode, nineCode } };

            var products = new List<Product>()
            {
                minge, papusa, bile, urs
            };

            var result = ProductFeatures.NotEvenOneFeature(products, features);
            var expectedResult = new List<Product>()
            {
                bile
            };

            Assert.Equal(expectedResult, result);
        }
    }
}
