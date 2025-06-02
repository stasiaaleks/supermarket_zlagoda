SELECT st.*, product_name, characteristics
FROM store_product st
INNER JOIN public.product p ON p.id_product = st.id_product
WHERE upc_prom IS NULL