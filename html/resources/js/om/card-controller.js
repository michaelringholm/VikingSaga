function CardController($scope) {
	$scope.cards = [
		{
			Id : 1,
			Title : 'Fox',
			ImgPath : './resources/images/fox-archer.jpg'
		},
		{
			Id : 2,
			Title : 'Bear',
			ImgPath : './resources/images/bear.jpg'
		},
				{
			Id : 3,
			Title : 'Bear',
			ImgPath : './resources/images/bear.jpg'
		},
				{
			Id : 4,
			Title : 'Bear',
			ImgPath : './resources/images/bear.jpg'
		},
		{
			Id : 5,
			Title : "Skeleton",
			ImgPath : './resources/images/skeleton.jpg'
		} 
	];
	
	$scope.handCards = [
		{
			Id : 6,
			Title : 'Fox',
			ImgPath : './resources/images/fox-archer.jpg'
		},
		{
			Id : 7,
			Title : 'Bear',
			ImgPath : './resources/images/bear.jpg'
		},
		{
			Id : 8,
			Title : 'Bear',
			ImgPath : './resources/images/bear.jpg'
		},
		{
			Id : 9,
			Title : 'Bear',
			ImgPath : './resources/images/bear.jpg'
		},
		{
			Id : 10,
			Title : "Skeleton",
			ImgPath : './resources/images/skeleton.jpg'
		} 
	];
		
	$scope.oppponentPartyCards = [
		{
			Id : 11,
			Title : 'Fox',
			ImgPath : './resources/images/fox-archer.jpg'
		},
		{
			Id : 12,
			Title : 'Bear',
			ImgPath : './resources/images/bear.jpg'
		},
		{
			Id : 13,
			Title : 'Bear',
			ImgPath : './resources/images/bear.jpg'
		},
				{
			Id : 14,
			Title : 'Bear',
			ImgPath : './resources/images/bear.jpg'
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