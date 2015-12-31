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
			Title : "Skeleton",
			ImgPath : './resources/images/skeleton.jpg'
		} 
	];
	
	$scope.handCards = [
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
			Title : "Skeleton",
			ImgPath : './resources/images/skeleton.jpg'
		} 
	];
	
	$scope.$watch('cardName', function() {
	    console.log('card changed!');
	    cardModel.initControls();
	});	
}