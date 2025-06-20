UPDATE store_product
SET upc_prom = @UPCProm,
    id_product = @IdProduct,
    selling_price = @SellingPrice,
    products_number = @ProductsNumber,
    promotional_product = @PromotionalProduct
WHERE upc = @UPC
RETURNING upc;
