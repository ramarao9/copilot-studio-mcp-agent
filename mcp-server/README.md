# Product MCP Server

A Model Context Protocol (MCP) server implementation using Azure Functions that provides product catalog and review information. This project demonstrates how to build an MCP-compatible server using the Azure Functions Worker Extensions for MCP.

## Overview

This MCP server exposes two main tools:
- **ListProducts**: Retrieve all products from the product catalog
- **GetProductReviews**: Get customer reviews for a specific product by product ID

The server is built using:
- .NET 8.0
- Azure Functions v4
- Microsoft.Azure.Functions.Worker.Extensions.Mcp (v1.0.0-preview.7)

## Project Structure

```
mcp-server/
├── Program.cs                      # Application entry point and DI configuration
├── ListProducts.cs                 # MCP tool for listing products
├── ProductReviews.cs               # MCP tool for getting product reviews
├── ToolsInformation.cs             # Tool metadata and descriptions
├── host.json                       # Azure Functions host configuration
├── local.settings.json             # Local development settings
├── openapi.yaml                    # OpenAPI specification for MCP integration
├── Models/                         # Data models
│   ├── Product.cs
│   ├── ProductReview.cs
│   └── GetProductReviewRequest.cs
├── Interfaces/                     # Service interfaces
│   ├── IProductService.cs
│   └── IProductReviewService.cs
├── Services/                       # Service implementations
│   ├── ProductService.cs
│   └── ProductReviewService.cs
└── data/                          # JSON data files
    ├── products.json
    └── product-reviews.json
```

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Azure Functions Core Tools](https://learn.microsoft.com/azure/azure-functions/functions-run-local) v4.x
- An IDE like Visual Studio Code or Visual Studio

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/ramarao9/copilot-studio-mcp-agent.git
cd copilot-studio-mcp-agent/mcp-server
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Project

```bash
dotnet build
```

## Running the Function App Locally

### Option 1: Using Azure Functions Core Tools

```bash
func start
```

The function app will start and display the local endpoint (typically `http://localhost:7071`).

### Option 2: Using VS Code Tasks

If you're using Visual Studio Code, you can use the pre-configured tasks:

1. Press `Cmd+Shift+P` (macOS) or `Ctrl+Shift+P` (Windows/Linux)
2. Select "Tasks: Run Task"
3. Choose "build (functions)" to build the project
4. Then run the "func: host start" task to start the function app

### Option 3: Using .NET CLI with Watch Mode

For development with auto-reload:

```bash
dotnet build
cd bin/Debug/net8.0
func start
```

## Testing the MCP Server

### Local Testing

Once the function app is running, the MCP endpoint will be available at:

```
http://localhost:7071/runtime/webhooks/mcp
```

### Testing with HTTP Client

#### 1. List All Products

Send a POST request to invoke the ListProducts tool:

```bash
curl -X POST http://localhost:7071/runtime/webhooks/mcp \
  -H "Content-Type: application/json" \
  -d '{
    "jsonrpc": "2.0",
    "method": "tools/call",
    "params": {
      "name": "ListProducts",
      "arguments": {}
    },
    "id": 1
  }'
```

#### 2. Get Product Reviews

Send a POST request to get reviews for a specific product:

```bash
curl -X POST http://localhost:7071/runtime/webhooks/mcp \
  -H "Content-Type: application/json" \
  -d '{
    "jsonrpc": "2.0",
    "method": "tools/call",
    "params": {
      "name": "GetProductReviews",
      "arguments": {
        "ProductId": 1
      }
    },
    "id": 2
  }'
```

### Testing with MCP Inspector

You can also test using the MCP Inspector tool:

```bash
npx @modelcontextprotocol/inspector
```

Then configure it to connect to your local MCP endpoint.

## MCP Tools Available

### ListProducts

**Description**: List all products from the product catalog.

**Parameters**: None

**Returns**: JSON array of products with the following structure:
```json
[
  {
    "id": 1,
    "name": "Product Name",
    "category": "Category",
    "price": 99.99,
    "description": "Product description"
  }
]
```

### GetProductReviews

**Description**: Get product reviews for a product using the product ID in the product catalog.

**Parameters**:
- `ProductId` (integer, required): The ID of the product to get reviews for

**Returns**: JSON array of reviews:
```json
[
  {
    "id": 1,
    "productId": 1,
    "rating": 5,
    "comment": "Great product!",
    "author": "Customer Name",
    "date": "2024-01-15"
  }
]
```

## Deployment to Azure

### Using Azure Functions Extension for VS Code

1. Install the [Azure Functions extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
2. Click the Azure icon in the sidebar
3. Sign in to your Azure account
4. Click "Deploy to Function App"
5. Follow the prompts to create a new Function App or select an existing one

### Using Azure CLI

```bash
# Login to Azure
az login

# Create a resource group
az group create --name ProductMCPResourceGroup --location eastus

# Create a storage account
az storage account create --name productmcpstorage --location eastus --resource-group ProductMCPResourceGroup --sku Standard_LRS

# Create a Function App
az functionapp create --resource-group ProductMCPResourceGroup --consumption-plan-location eastus --runtime dotnet-isolated --functions-version 4 --name ProductMCPServer --storage-account productmcpstorage

# Deploy the function app
func azure functionapp publish ProductMCPServer
```

### Post-Deployment Configuration

After deployment, your MCP endpoint will be available at:

```
https://<your-function-app-name>.azurewebsites.net/runtime/webhooks/mcp
```

Update the `openapi.yaml` file with your actual function app name and client ID for authentication.

## Integration with Copilot Studio

The included `openapi.yaml` file can be used to integrate this MCP server with Microsoft Copilot Studio:

1. Deploy the function app to Azure
2. Update the `openapi.yaml` with your function app name and authentication details
3. In Copilot Studio, add a new connector using the OpenAPI specification
4. Configure OAuth2 authentication as specified in the OpenAPI file

## Data Files

The server uses JSON files for data storage located in the `data/` directory:

- `products.json`: Contains the product catalog
- `product-reviews.json`: Contains product reviews

These files are copied to the output directory during build and can be modified to update the data.

## Development

### Adding New Tools

To add a new MCP tool:

1. Create a new function class (e.g., `NewTool.cs`)
2. Add the tool metadata to `ToolsInformation.cs`
3. Implement the function with `[McpToolTrigger]` attribute
4. Register any required services in `Program.cs`

### Modifying Tool Descriptions

Tool names and descriptions are defined in `ToolsInformation.cs`. Update these constants to modify how the tools are presented to MCP clients.

## Troubleshooting

### Function App Not Starting

- Ensure .NET 8.0 SDK is installed: `dotnet --version`
- Verify Azure Functions Core Tools is installed: `func --version`
- Check for port conflicts (default port 7071)

### MCP Endpoint Not Responding

- Verify the function app is running
- Check the logs for any errors
- Ensure the MCP extension is properly installed (check `ProductsMCP.csproj`)

### Build Errors

- Clean and rebuild: `dotnet clean && dotnet build`
- Restore NuGet packages: `dotnet restore`


## Support

For issues and questions, please [open an issue](https://github.com/ramarao9/copilot-studio-mcp-agent/issues) on GitHub.
