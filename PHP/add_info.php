<?php
require "init.php";

$name = $_POST["name"];
$user_name = $_POST["user_name"];
$user_pass = $_POST["user_pass"];
$email = $_POST["email"];
$mobile = $_POST["mobile"];

$sql = "insert into user_info values('$name','$user_name','$user_pass','$email','$mobile');";

if(mysqli_query($con,$sql)){
	
	echo "<br><h3>One Row inserted...</h3>";
	
}else{
	echo "Error in insertion...".mysqli_error($con);
}

mysqli_close($con);
?>