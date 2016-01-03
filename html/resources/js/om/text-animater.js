function TextAnimater(imgSrc, animatedText, baseCanvas, hoverCanvas) {
	var _this = this;
	
	this.ctx;
	this.step = 10;
	this.steps = 50;
	this.delay = 20;
	this.imgSrc = imgSrc;
	this.animatedText = animatedText;
	this.hoverCanvas = hoverCanvas;
	this.baseCanvas = baseCanvas;
		 
	this.initUnderlyingImg = function() {
		var context = _this.baseCanvas.getContext("2d");
		var imageObj = new Image();
		imageObj.src = _this.imgSrc;
		imageObj.onload = function() {
			context.drawImage(imageObj, 0, 0, 300, 200);
		};

		//context.rotate(rotation * Math.PI / 180);
	};
	
	this.initAnimatedText = function() {
		_this.ctx = this.hoverCanvas.getContext("2d");
		_this.ctx.fillStyle = "yellow";
		_this.ctx.font = "10pt Helvetica";
		_this.ctx.textAlign = "center";
		_this.ctx.textBaseline = "middle";				
	};
	
	this.runAnimation = function() {				
		_this.step++;
		_this.ctx.clearRect(0, 0, _this.hoverCanvas.width, _this.hoverCanvas.height);
		_this.ctx.save();
		_this.ctx.translate(_this.hoverCanvas.width / 2, _this.hoverCanvas.height / 2);
		_this.ctx.font = _this.step + "pt Helvetica";
		_this.ctx.fillText(_this.animatedText, 0, 0);
		_this.ctx.restore();
		
		if (_this.step < _this.steps)
			var t = setTimeout(_this.runAnimation, 20);
		else
			_this.ctx.clearRect(0, 0, _this.hoverCanvas.width, _this.hoverCanvas.height);
	};
	
	this.construct = function() {
		_this.initAnimatedText();
		_this.initUnderlyingImg();
	};
	
	_this.construct();

}