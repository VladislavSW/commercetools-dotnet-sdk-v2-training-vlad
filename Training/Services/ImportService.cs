using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Base.Client;
using commercetools.Sdk.ImportApi.Models.Common;
using commercetools.Sdk.ImportApi.Models.Importcontainers;
using commercetools.Sdk.ImportApi.Models.Importoperations;
using commercetools.Sdk.ImportApi.Models.Importrequests;
using commercetools.Sdk.ImportApi.Models.Importsummaries;
using commercetools.Sdk.ImportApi.Models.Productdrafts;
using commercetools.Sdk.ImportApi.Models.Productvariants;
using commercetools.Sdk.ImportApi.Extensions;
using System.Linq;
using commercetools.Sdk.ImportApi.Models.Products;

namespace Training.Services
{
    public class ImportService
    {
        private readonly IClient _importClient;
        private readonly CSVHelper _csvHelper;
        private readonly string _projectKey;
        private const string PREFIX = "MG";

        public ImportService(IClient client, string projectKey)
        {
            _importClient = client;
            _projectKey = projectKey;
            _csvHelper = new CSVHelper();
        }

        
        /// <summary>
        /// Creates an ImportContainer
        /// </summary>
        /// <param name="importContainerDraft"></param>
        /// <returns></returns>
        public async Task<IImportContainer> CreateImportContainer(ImportContainerDraft importContainerDraft)
        {
            IImportContainer importContainer = await _importClient.WithImportApi(_projectKey)
                .ImportContainers()
                .Post(importContainerDraft)
                .ExecuteAsync();

            return importContainer;
        }

        /// <summary>
        /// GET import summary for the import container
        /// </summary>
        /// <param name="importContainerKey"></param>
        /// <returns></returns>
        public async Task<IImportSummary> GetImportContainerSummary(string importContainerKey)
        {
            IImportSummary importSummary = await _importClient.WithImportApi(_projectKey)
                .ImportContainers()
                .WithImportContainerKeyValue(importContainerKey)
                .ImportSummaries()
                .Get()
                .ExecuteAsync();

            return importSummary;
        }

        /// <summary>
        /// GET import operations for the import container
        /// </summary>
        /// <param name="importContainerKey"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public async Task<IImportOperationPagedResponse> GetImportOperationsByImportContainer(string importContainerKey, 
            bool debug)
        {

            IImportOperationPagedResponse importOperationPagedResponse = await _importClient.WithImportApi(_projectKey)
                .ImportContainers()
                .WithImportContainerKeyValue(importContainerKey)
                .ImportOperations()
                .Get()
                .ExecuteAsync();

            return importOperationPagedResponse;
        }

        /// <summary>
        /// GET import operation status for the import operation by id
        /// </summary>
        /// <param name="operationId"></param>
        /// <returns></returns>
        public async Task<IImportOperation> CheckImportOperationStatus(string operationId)
        {
            IImportOperation importOperation = await _importClient.WithImportApi(_projectKey)
                .ImportOperations()
                .WithIdValue(operationId)
                .Get()
                .ExecuteAsync();

            return importOperation;
        }

        /// <summary>
        /// Import products from the csv file
        /// </summary>
        /// <param name="importContainerKey"></param>
        /// <param name="csvFile"></param>
        /// <returns></returns>
        public async Task<IImportResponse> ImportProducts(string importContainerKey, string csvFile)
        {
            List<IProductDraftImport> productDraftImports = this.GetProductDraftImportList(csvFile);
            IList<IProductImport> productImports = new List<IProductImport>();
            foreach (var productDraft in productDraftImports) {
                productImports.Add(
                    new ProductImport()
                    {
                        Key = productDraft.Key,
                        Name = productDraft.Name,
                        ProductType = productDraft.ProductType,
                        Slug = productDraft.Slug
                    }
                );
            }
            IProductImportRequest productImportRequest = new ProductImportRequest()
            {
                    Resources = productImports
            };
            IImportResponse importResponse = await _importClient.WithImportApi(_projectKey)
                .Products()
                .ImportContainers()
                .WithImportContainerKeyValue(importContainerKey)
                .Post(productImportRequest)
                .ExecuteAsync();

            return importResponse;
        }

        #region Helpers

        private List<IProductDraftImport> GetProductDraftImportList(string fileName)
        {
            var listOfCsvProducts = ParseCsvFile(fileName);
            var listOfProductDraftImport = listOfCsvProducts.Select(product => new ProductDraftImport
                {
                    Key = PREFIX + "-" + product.ProductName,
                    Name = new LocalizedString { { "en", product.ProductName }, { "de", product.ProductName } },
                    ProductType = new ProductTypeKeyReference { Key = product.ProductType },
                    Slug = new LocalizedString { { "en", PREFIX + "-" + product.ProductName }, { "de", PREFIX + "-" + product.ProductName } },
                    MasterVariant = new ProductVariantDraftImport
                    {
                        Sku = PREFIX + "-" + product.InventoryId,
                        Key = (PREFIX + "-" + product.ProductName).ToLower(),
                        Images = new List<IImage>
                        {
                            new Image {Url = product.ImageUrl, Dimensions = new AssetDimensions {W = 177, H = 237}}
                        },
                        Attributes = new List<IAttribute>
                        {
                            new TextAttribute { Name = "phonecolor", Value = product.Color },
                            new NumberAttribute { Name = "phoneweight", Value = product.Weight }
                        }
                    }
                
                });

            return listOfProductDraftImport.ToList<IProductDraftImport>();
        }

        private List<CSVProduct> ParseCsvFile(string file)
        {
            var list = _csvHelper.GetData<CSVProduct>(file);
            return list;
        }

        #endregion
        
    }

    public class CSVProduct
    {
        public string ProductType { get; set; }
        public string ProductId { get; set; }
        public string InventoryId { get; set; }
        public string ArticleId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string BasePrice { get; set; }
        public string CurrencyCode { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public decimal Weight { get; set; }
    }
}