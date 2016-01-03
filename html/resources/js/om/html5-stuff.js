//////////// HTML5 Image Rotation ////////////////////////

function drawImage(canvas, imgSrc, rotation, leftPos, topPos, width, height) {
	var context = canvas.getContext("2d");

	var imageObj = new Image();
	imageObj.src = imgSrc;
	imageObj.onload = function() {
		context.drawImage(imageObj, leftPos, topPos, width, height);
	};

	context.rotate(rotation * Math.PI / 180);
}

//////////////// HTML5 Animated Text ///////////////////////
// http://www.authorcode.com/text-animation-in-html5/




///////////// HTML5 Audio API //////////////////////
// http://www.freesfx.co.uk/sfx/dropping

function playSound(soundSrc) {
	var snd = new Audio(soundSrc); // buffers automatically when created
	snd.play();
}
