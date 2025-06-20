SELECT st.upc, st.products_number, st.selling_price, p.product_name, p.characteristics
FROM store_product st
INNER JOIN public.product p ON p.id_product = st.id_product
WHERE st.upc = @UPC;