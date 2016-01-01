var cardModel = new CardModel();
cardModel.initControls();

function CardModel() {
	this.initControls = function() {
		console.log('cardModel.initControls() called!')
	}
}


new AngularJSHelper();


function AngularJSHelper() {
	// Angular code fails if inside jQuery ready block
	if(angular) {
		console.log("AngularJS is initialized!");
		var cardModule = angular.module('cardModule', []);
		cardModule.controller("CardController", CardController);
	}
	else
		console.log("AngularJS not yet initialized!");

}


///JQUERY

$(function() {
	$("canvas").each(function() {
		drawImage(this, $(this).attr("data-img-src"), $(this).attr("data-rotation"), 20, 20, 100, 100);
	});
});



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


//////////////////////////// DRAG N DROP /////////////////////////////
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
    console.log($(card.html()));
    console.log("drop complete!");
}