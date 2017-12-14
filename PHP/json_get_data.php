<?php

require "init.php";
$sql = "select * from user_info";

$result = mysqli_query($con,$sql);

$response = array();

while($row = mysqli_fetch_array($result)){
	array_push($response,array("name"=>$row[0],"user_name"=>$row[1],"user_pass"=>$row[2],"email"=>$row[3],"mobile"=>$row[4]));
	//array_push($response,$row[0]);
}

echo json_encode(array("server_response"=>$response));
mysqli_close($con);

?>