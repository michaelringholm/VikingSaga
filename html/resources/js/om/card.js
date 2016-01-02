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
	$(".partyCards canvas").each(function() {
		drawImage(this, $(this).attr("data-img-src"), $(this).attr("data-rotation"), 20, 10, 230, 120);
	});
	
	$(".oppponentPartyCards canvas").each(function() {
		drawImage(this, $(this).attr("data-img-src"), $(this).attr("data-rotation"), 20, 10, 230, 120);
	});
	
	$(".handCards canvas").each(function() {
		drawImage(this, $(this).attr("data-img-src"), $(this).attr("data-rotation"), 20, 10, 230, 120);
	});
});