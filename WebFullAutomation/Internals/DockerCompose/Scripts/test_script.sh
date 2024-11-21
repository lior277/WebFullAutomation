#!/bin/bash
docker exec -it dockercompose-rabbitmq-1 bash -c \
"echo '#!/bin/bash' > /docker-entrypoint-initdb.d/test_script.sh && \
echo 'echo \"Test script executed successfully\" | tee -a /var/log/test-script.log' >> /docker-entrypoint-initdb.d/test_script.sh && \
chmod +x /docker-entrypoint-initdb.d/test_script.sh"