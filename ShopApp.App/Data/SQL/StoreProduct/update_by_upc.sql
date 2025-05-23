UPDATE store_product
SET upc_prom = @UPCProm,
    id_product = @ProductId,
    selling_price = @SellingPrice,
    products_number = @ProductsNumber,
    promotional_product = @PromotionalProduct
WHERE upc = @UPC;
