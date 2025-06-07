SELECT st.*, product_name, characteristics
FROM store_product st
INNER JOIN public.product p ON p.id_product = st.id_product
WHERE st.upc_prom IS NULL AND st.upc NOT IN (
    SELECT st_prom.upc_prom
    FROM store_product st_prom
    WHERE st_prom.upc_prom IS NOT NULL
)