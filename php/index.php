<?php
$content = "some text here";

showAllParams();

$post = file_get_contents('php://input');
if($post==null)
	echo "No post data found";
else
	echo "Post data found";

$content .= "\nMore content 2";
saveFile($content);


echo "Done saving file";



function showAllParams()
{
	//echo $_GET . "<br/>";
	//echo $_POST . "<br/>";
	echo "GET data:";
	print_r($_GET);
	echo "<br/>";
	echo "POST data:";
	print_r($_POST);
	echo "<br/>";
	echo "FILES data:";
	print_r($_FILES);
	echo "<br/>";

foreach($_GET as $key=>$value){
	echo $key, ' => ', $value, "<br/>";
}

foreach ($_POST as $key2 => $value2) {
    //do something
    echo $key2 . ' has the value of ' . $value2;
}
}

function saveFile($fileContents)
{
$fp = fopen($_SERVER['DOCUMENT_ROOT'] . "/viking-saga/ethlore.txt","wb");
fwrite($fp,$fileContents);
fclose($fp);
}

?>


<html>
<body>
Saving file
</body>
</html>