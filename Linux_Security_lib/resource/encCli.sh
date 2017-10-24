while true; do read -n30 ui; echo $ui | openssl enc -aes-256-ctr -a -k PaSSw;
done | nc localhost 8877 | while read so; do decoded_so=`echo "$so" | openssl enc -d -a -aes-256-ctr -k PaSSw`;
echo -e "Incoming: $decoded_so"; done