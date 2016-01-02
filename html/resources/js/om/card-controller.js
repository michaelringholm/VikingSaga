function CardController($scope) {
	$scope.partyCards = [
		{
			Id : 1,
			Title : "Fox archer",
			ImgPath : './resources/images/fox-archer.jpg'
		},
		{
			Id : 2,
			Title : "Fox archer",
			ImgPath : './resources/images/fox-archer.jpg'
		},
				{
			Id : 3,
			Title : "Fox archer",
			ImgPath : './resources/images/fox-archer.jpg'
		},
				{
			Id : 4,
			Title : "Fox archer",
			ImgPath : './resources/images/fox-archer.jpg'
		},
		{
			Id : 5,
			Title : "Bear",
			ImgPath : './resources/images/bear.jpg'
		} 
	];
	
	$scope.handCards = [
		{
			Id : 6,
			Title : "Tiny Healing Potion",
			ImgPath : './resources/images/abilities/healing-potion.jpg',
			UsageText : 'Healed!'
		},
		{
			Id : 7,
			Title : "Tiny Healing Potion",
			ImgPath : './resources/images/abilities/healing-potion.jpg',
			UsageText : 'Healed!'
		},
		{
			Id : 8,
			Title : "Tiny Healing Potion",
			ImgPath : './resources/images/abilities/healing-potion.jpg',
			UsageText : 'Healed!'
		},
		{
			Id : 9,
			Title : "Tiny Healing Potion",
			ImgPath : './resources/images/abilities/healing-potion.jpg',
			UsageText : 'Healed!'
		},
		{
			Id : 10,
			Title : "Tiny Healing Potion",
			ImgPath : './resources/images/abilities/healing-potion.jpg',
			UsageText : 'Healed!'
		} 
	];
		
	$scope.oppponentPartyCards = [
		{
			Id : 11,
			Title : "Skeleton",
			ImgPath : './resources/images/skeleton.jpg'
		},
		{
			Id : 12,
			Title : "Skeleton",
			ImgPath : './resources/images/skeleton.jpg'
		},
		{
			Id : 13,
			Title : "Skeleton",
			ImgPath : './resources/images/skeleton.jpg'
		},
				{
			Id : 14,
			Title : "Skeleton",
			ImgPath : './resources/images/skeleton.jpg'
		},
		{
			Id : 15,
			Title : "Skeleton",
			ImgPath : './resources/images/skeleton.jpg'
		} 
	];
	
	$scope.$watch('cardName', function() {
	    console.log('card changed!');
	    cardModel.initControls();
	});	
}