# Reenbit.IMS
This repository contains a set of RESTful APIs for invoice management operations. It represents 'Invoice' as a target document and all the relevant CRUDs exposed via APIs. In addition, this repo includes one of possible solutions for document audit tracking, for this purpose Azure Function has been implemented based on the same 'Invoice' domain entity.

## Swagger page

![Swagger page](/Images/swagger-page.PNG)

## Technology stack
ASP.NET Core, Azure Cosmos DB, Azure Functions (Cosmos DB trigger)

## Demo

- #### User 1 creates an initial invoice document
  ![Create invoice request](/Images/CreateInvoiceRequest.PNG)
  
  ![Create invoice response](/Images/CreateInvoiceResponse.PNG)
  
  ![Created invoice](/Images/CreatedInvoiceInDb.PNG)
  
- #### User 1 updates previously created invoice
  ![Update invoice by user 1](/Images/UpdateInvoiceByUser1.PNG)
- #### User 2 updates the same invoice
  ![Update invoice by user 2](/Images/UpdateInvoiceByUser2.PNG)
- #### Azure Function output (two 'TrackInvoiceUpdates' events handled)
  ![Azure Function Output](/Images/AzureFunctionOutput.PNG)
- #### Audit log results in database
  ![Audit Log 1](/Images/AuditLog1.PNG)
  ![Audit Log 2](/Images/AuditLog2.PNG)
 
## Notes
- **"actor-id"** request header is passed to APIs just for demo purposes (to simulate multiple users)
- all audit log records are collected into separate Cosmos DB container
- only **last update log** information is a part of 'Invoice' document, which prevents from 
keeping a 'huge' document as a result of many updates   
