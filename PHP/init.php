<?php

$host = "mysql11.000webhost.com";
$user = "a3952107_darren";
$password = "Xzmxhl921";
$db = "a3952107_scsu";

$con = mysqli_connect($host,$user,$password,$db);

if(!$con){
		die("Error in connection ".mysqli_connect_error());
}else{
	//echo "<br><h3>Connection Success....</h3>";
}

?>