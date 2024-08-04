﻿using EDIConverterWeb.Data;
using EDIConverterWeb.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EDIConverterWeb.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EDIConverterController : ControllerBase
    {
        private string _connectionString;
        public EDIConverterController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpPost]
        [Route("create855")]
        public _855IdViewModel Create855(Create855ViewModel vm)
        {
            var repo = new PurchaseOrderAcknowledgementRepo(_connectionString);
            var isValidInfo = repo.IsValid855Data(vm.PurchaseOrderNumber, vm.PurchaseOrderDate, vm.TestIndicator);
            if (isValidInfo)
            {
                //before i enter the info, make it automatically delete the extra spaces from the po1 lines
                var id = repo.AddDoc(vm.PurchaseOrderNumber, vm.PurchaseOrderDate, vm.TestIndicator, vm.ItemsList);
                return new _855IdViewModel
                {
                    Id = id
                };
            }
            return new _855IdViewModel
            {
                Id = 0
            };
        }

        [HttpGet]
        [Route("view855/{id}")]
        public View855ViewModel View855(int id)
        {
            var repo = new EDIFileWriterRepo(_connectionString);
            return new()
            {
                EdiText = repo.WritePurchaseOrderAcknowledgement(id)
            };
        }
    }
}