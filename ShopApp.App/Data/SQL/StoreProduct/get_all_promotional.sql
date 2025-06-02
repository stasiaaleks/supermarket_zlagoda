SELECT st_prom.*, product_name, characteristics
FROM store_product st
INNER JOIN product p ON p.id_product = st.id_product
INNER JOIN store_product st_prom ON st.upc_prom = st_prom.upc