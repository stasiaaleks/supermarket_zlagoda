SELECT st_prom.*
FROM store_product st
         INNER JOIN store_product st_prom ON st.upc_prom = st_prom.upc
WHERE st.upc = @UPC;