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


////////////////////////////HTML5 DRAG N DROP /////////////////////////////
function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
	var cardId = $(ev.currentTarget).attr("data-card-id");
    ev.dataTransfer.setData("text", cardId);
	//ev.dataTransfer.setData("text", ev.target.id);
	console.log("drag complete!");
}

function drop(ev) {
    ev.preventDefault();
    var cardId = ev.dataTransfer.getData("text");
    var card = $(".card[data-card-id=" + cardId + "]");
    //ev.target.appendChild(document.getElementById(data));
    $(ev.target).append(card);
	
	
	playSound('./resources/sounds/drop.mp3');
    console.log($(card.html()));	
    console.log("drop complete!");
}