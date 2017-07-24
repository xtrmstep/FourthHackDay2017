using AzureService;
using Example;
using Example.SampleResponseJsonTypes;
using FourthMLInsights.Models;
using MLFormulas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FourthMLInsights.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult Create(SaleDataInputModel inputModel)
        {
            if (!ModelState.IsValid)
                RedirectToAction("Index");

            var model = GetModel(inputModel);

            return View(model);
        }

        private PredictionViewModel GetModel(SaleDataInputModel inputModel)
        {
            var displayModel = new PredictionViewModel();

            SetMlDisplayPredictionData(displayModel, inputModel);

            SetFourthDisplayPredictionData(displayModel);

            return displayModel;
        }

        private static void SetFourthDisplayPredictionData(PredictionViewModel model)
        {
            //Garlic Bread first week of May data
            double[] garlicBreadMay = new double[] { 3, 3, 2, 2, 0, 0, 0 };
            //Garlic Bread April data
            double[] estimated = new double[] { 2, 3, 5, 5, 2, 1, 2, 1, 2 };
            //double[] estimated = new double[] { 1, 1, 1, 1, 1, 1, 1 };


            model.FourthMeanAbsoluteError = MLMathFormulas.MeanAbsoluteError(garlicBreadMay, estimated);
            model.FourthMeanSquaredError = MLMathFormulas.MeanSquaredError(garlicBreadMay, estimated);
            model.FourthCoefficientOfDetermination = Math.Round(MLMathFormulas.CoefficientOfDetermination(garlicBreadMay, estimated), 6);
        }

        private static void SetMlDisplayPredictionData(PredictionViewModel displayModel, SaleDataInputModel inputModel)
        {
            var uri = Properties.Settings.Default.Uri;
            var key = Properties.Settings.Default.Key;

            var client = new SalesDemandService(uri, key);

            var estimatedSales = new List<double>();
            double[] garlicBreadMay = new double[] { 3, 3, 2, 2, 0, 0, 0 };

            var tempDate = inputModel.StartDate;
            
            while (tempDate <= inputModel.EndDate)
            {

                var data = new Dictionary<string, string>()
                {
                    { "Locationid", "69"},
                    { "RecipeName", ""},
                    { "PLU", inputModel.PLU }, // "2549"},
                    { "Salesdate", tempDate.ToString()},
                    { "Quantity","1"},
                    { "NetSalesPrice", "1"},
                    { "CostPrice", "1" },
                    { "Year", tempDate.Year.ToString()},
                    { "Month", tempDate.Month.ToString()},
                    { "Day",tempDate.Day.ToString()},
                    { "WeekDay","1"},
                    { "YearDay","1"}
                };

                var task = client.GetPrediction1(data);
                task.Wait();
                var predictionResult = GetScoredLabelsValue(task.Result);
                estimatedSales.Add(predictionResult);

                tempDate = tempDate.AddDays(1);
            }
            
            displayModel.MeanAbsoluteError = MLMathFormulas.MeanAbsoluteError(garlicBreadMay, estimatedSales.ToArray());
            displayModel.MeanSquaredError = MLMathFormulas.MeanSquaredError(garlicBreadMay, estimatedSales.ToArray());
            var coeff = MLMathFormulas.CoefficientOfDetermination(garlicBreadMay, estimatedSales.ToArray());
            displayModel.CoefficientOfDetermination = 
                //coeff > 0
                //? Math.Round(coeff, 6)
                //: 
            estimatedSales.First();
        }

        private static double GetScoredLabelsValue(string response)
        {
            var pred = JsonConvert.DeserializeObject<SampleResponse>(response);

            var result = pred.Results.Output1[0].ScoredLabels;
            
            return Convert.ToDouble(result);
        }
    }
}