INSERT INTO store_product (
    upc, upc_prom, id_product, selling_price, products_number, promotional_product
)
VALUES (
           @UPC, @UPCProm, @ProductId, @SellingPrice, @ProductsNumber, @PromotionalProduct
       )
RETURNING upc;
