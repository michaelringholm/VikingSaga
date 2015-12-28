<?php

$fileContents  = getRequestParam("data");
$profileName  = getRequestParam("profile_name");
saveFile($profileName,$fileContents);
//echo("Done saving file");
echo($fileContents);

function getRequestParam($key)
{
	return $_GET[$key];
}

function saveFile($profileName, $fileContents)
{
	//echo("File contents=" . $fileContents);
	$filePath = $_SERVER['DOCUMENT_ROOT'] . "/viking-saga/" . $profileName . ".txt";
	$myfile = fopen($filePath,"wb") or die("Unable to open file!");
	fwrite($myfile, $fileContents);
	fclose($myfile);
}

?>