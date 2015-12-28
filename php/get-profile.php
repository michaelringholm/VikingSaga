<?php

$profileName = getRequestParam("profile_name");
$fileContents = loadFile($profileName);
echo($fileContents);

function getRequestParam($key)
{
	return $_GET[$key];
}

function loadFile($profileName)
{
	$filePath = $_SERVER['DOCUMENT_ROOT'] . "/viking-saga/" . $profileName . ".txt";
$myfile = fopen($filePath,"r") or die("Unable to open file!");
$fileContents = fread($myfile,filesize($filePath));
fclose($myfile);

return $fileContents;
}

function saveFile($fileContents)
{
$fp = fopen($_SERVER['DOCUMENT_ROOT'] . "/viking-saga/ethlore.txt","wb");
fwrite($fp,$fileContents);
fclose($fp);
}

?>