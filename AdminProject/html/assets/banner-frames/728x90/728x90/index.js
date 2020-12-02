(function (lib, img, cjs, ss, an) {

var p; // shortcut to reference prototypes
lib.webFontTxtInst = {}; 
var loadedTypekitCount = 0;
var loadedGoogleCount = 0;
var gFontsUpdateCacheList = [];
var tFontsUpdateCacheList = [];
lib.ssMetadata = [];



lib.updateListCache = function (cacheList) {		
	for(var i = 0; i < cacheList.length; i++) {		
		if(cacheList[i].cacheCanvas)		
			cacheList[i].updateCache();		
	}		
};		

lib.addElementsToCache = function (textInst, cacheList) {		
	var cur = textInst;		
	while(cur != exportRoot) {		
		if(cacheList.indexOf(cur) != -1)		
			break;		
		cur = cur.parent;		
	}		
	if(cur != exportRoot) {		
		var cur2 = textInst;		
		var index = cacheList.indexOf(cur);		
		while(cur2 != cur) {		
			cacheList.splice(index, 0, cur2);		
			cur2 = cur2.parent;		
			index++;		
		}		
	}		
	else {		
		cur = textInst;		
		while(cur != exportRoot) {		
			cacheList.push(cur);		
			cur = cur.parent;		
		}		
	}		
};		

lib.gfontAvailable = function(family, totalGoogleCount) {		
	lib.properties.webfonts[family] = true;		
	var txtInst = lib.webFontTxtInst && lib.webFontTxtInst[family] || [];		
	for(var f = 0; f < txtInst.length; ++f)		
		lib.addElementsToCache(txtInst[f], gFontsUpdateCacheList);		

	loadedGoogleCount++;		
	if(loadedGoogleCount == totalGoogleCount) {		
		lib.updateListCache(gFontsUpdateCacheList);		
	}		
};		

lib.tfontAvailable = function(family, totalTypekitCount) {		
	lib.properties.webfonts[family] = true;		
	var txtInst = lib.webFontTxtInst && lib.webFontTxtInst[family] || [];		
	for(var f = 0; f < txtInst.length; ++f)		
		lib.addElementsToCache(txtInst[f], tFontsUpdateCacheList);		

	loadedTypekitCount++;		
	if(loadedTypekitCount == totalTypekitCount) {		
		lib.updateListCache(tFontsUpdateCacheList);		
	}		
};
// symbols:



(lib._1 = function() {
	this.initialize(img._1);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,728,90);


(lib._11 = function() {
	this.initialize(img._11);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,178,79);


(lib._13 = function() {
	this.initialize(img._13);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,28,28);


(lib._15 = function() {
	this.initialize(img._15);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,240,41);


(lib._16 = function() {
	this.initialize(img._16);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,221,49);


(lib._2 = function() {
	this.initialize(img._2);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,137,171);


(lib._3 = function() {
	this.initialize(img._3);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,51,51);


(lib._4 = function() {
	this.initialize(img._4);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,246,29);


(lib._5 = function() {
	this.initialize(img._5);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,272,46);


(lib._6 = function() {
	this.initialize(img._6);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,196,43);


(lib._7 = function() {
	this.initialize(img._7);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,728,90);


(lib._8 = function() {
	this.initialize(img._8);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,279,73);


(lib._9 = function() {
	this.initialize(img._9);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,223,48);


(lib.bigPizza = function() {
	this.initialize(img.bigPizza);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,229,151);


(lib.kola = function() {
	this.initialize(img.kola);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,226,103);


(lib.nefisKenar = function() {
	this.initialize(img.nefisKenar);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,136,78);


(lib.pzza = function() {
	this.initialize(img.pzza);
}).prototype = p = new cjs.Bitmap();
p.nominalBounds = new cjs.Rectangle(0,0,220,192);// helper functions:

function mc_symbol_clone() {
	var clone = this._cloneProps(new this.constructor(this.mode, this.startPosition, this.loop));
	clone.gotoAndStop(this.currentFrame);
	clone.paused = this.paused;
	clone.framerate = this.framerate;
	return clone;
}

function getMCSymbolPrototype(symbol, nominalBounds, frameBounds) {
	var prototype = cjs.extend(symbol, cjs.MovieClip);
	prototype.clone = mc_symbol_clone;
	prototype.nominalBounds = nominalBounds;
	prototype.frameBounds = frameBounds;
	return prototype;
	}


(lib.Tween30 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._15();
	this.instance.parent = this;
	this.instance.setTransform(121,-87);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(121,-87,240,41);


(lib.Tween28 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._16();
	this.instance.parent = this;
	this.instance.setTransform(-110.5,-24.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-110.5,-24.5,221,49);


(lib.Tween26 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._13();
	this.instance.parent = this;
	this.instance.setTransform(-14,-14);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-14,-14,28,28);


(lib.Tween23 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib.kola();
	this.instance.parent = this;
	this.instance.setTransform(-113,-51.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-113,-51.5,226,103);


(lib.Tween22 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib.pzza();
	this.instance.parent = this;
	this.instance.setTransform(-110,-96);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-110,-96,220,192);


(lib.Tween21 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib.pzza();
	this.instance.parent = this;
	this.instance.setTransform(-110,-96);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-110,-96,220,192);


(lib.Tween20 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib.bigPizza();
	this.instance.parent = this;
	this.instance.setTransform(133,-128,0.8,0.8);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(133,-128,183.2,120.8);


(lib.Tween18 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._11();
	this.instance.parent = this;
	this.instance.setTransform(-89,-39.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-89,-39.5,178,79);


(lib.Tween16 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._8();
	this.instance.parent = this;
	this.instance.setTransform(-139.5,-36.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-139.5,-36.5,279,73);


(lib.Tween14 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._9();
	this.instance.parent = this;
	this.instance.setTransform(123,-86);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(123,-86,223,48);


(lib.Tween12 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._7();
	this.instance.parent = this;
	this.instance.setTransform(-150,-125);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-150,-125,728,90);


(lib.Tween11 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._7();
	this.instance.parent = this;
	this.instance.setTransform(-150,-125);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-150,-125,728,90);


(lib.Tween10 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._5();
	this.instance.parent = this;
	this.instance.setTransform(-136,-23);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-136,-23,272,46);


(lib.Tween9 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._5();
	this.instance.parent = this;
	this.instance.setTransform(-136,-23);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-136,-23,272,46);


(lib.Tween8 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._6();
	this.instance.parent = this;
	this.instance.setTransform(-98,-21.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-98,-21.5,196,43);


(lib.Tween7 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._6();
	this.instance.parent = this;
	this.instance.setTransform(-98,-21.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-98,-21.5,196,43);


(lib.Tween6 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._4();
	this.instance.parent = this;
	this.instance.setTransform(-123,-14.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-123,-14.5,246,29);


(lib.Tween5 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._4();
	this.instance.parent = this;
	this.instance.setTransform(-123,-14.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-123,-14.5,246,29);


(lib.Tween3 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._3();
	this.instance.parent = this;
	this.instance.setTransform(-25.5,-25.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-25.5,-25.5,51,51);


(lib.Tween2 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._2();
	this.instance.parent = this;
	this.instance.setTransform(-68.5,-85.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-68.5,-85.5,137,171);


(lib.Tween1 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib._2();
	this.instance.parent = this;
	this.instance.setTransform(-68.5,-85.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-68.5,-85.5,137,171);


(lib.Symbol8 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib.nefisKenar();
	this.instance.parent = this;

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(1));

}).prototype = getMCSymbolPrototype(lib.Symbol8, new cjs.Rectangle(0,0,136,78), null);


(lib.Symbol10 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 2
	this.instance = new lib.Tween30("synched",0);
	this.instance.parent = this;
	this.instance.setTransform(183.4,73.2,0.7,0.7);
	this.instance._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(5).to({_off:false},0).to({scaleX:1,scaleY:1,x:111,y:92.5},7).wait(6).to({startPosition:0},0).to({scaleX:1.2,scaleY:1.2,x:62.8,y:106.9},4).to({scaleX:1,scaleY:1,x:111,y:92.5},3).wait(9).to({startPosition:0},0).to({scaleX:1.2,scaleY:1.2,x:62.8,y:106.9},4).to({scaleX:1,scaleY:1,x:111,y:92.5},3).to({_off:true},24).wait(57));

	// Layer 1
	this.instance_1 = new lib.Tween28("synched",0);
	this.instance_1.parent = this;
	this.instance_1.setTransform(110.5,24.5,0.7,0.7);
	this.instance_1.alpha = 0.051;

	this.timeline.addTween(cjs.Tween.get(this.instance_1).to({scaleX:1,scaleY:1,alpha:1},8).to({_off:true},57).wait(57));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(33.1,7.3,154.7,34.3);


(lib.Symbol9 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 4
	this.instance = new lib.Tween23("synched",0);
	this.instance.parent = this;
	this.instance.setTransform(679.2,38.3,0.8,0.8);
	this.instance._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(11).to({_off:false},0).to({x:274.5},8,cjs.Ease.get(0.96)).wait(57).to({startPosition:0},0).to({_off:true},5).wait(35));

	// Layer 3
	this.instance_1 = new lib.Tween26("synched",0);
	this.instance_1.parent = this;
	this.instance_1.setTransform(158.9,40.2);
	this.instance_1.alpha = 0.051;
	this.instance_1._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance_1).wait(8).to({_off:false},0).to({alpha:1},3,cjs.Ease.get(0.98)).wait(63).to({x:72,y:106},0).to({alpha:0},3,cjs.Ease.get(1)).to({_off:true},4).wait(35));

	// Layer 2
	this.instance_2 = new lib.Tween21("synched",0);
	this.instance_2.parent = this;
	this.instance_2.setTransform(-260,81);

	this.instance_3 = new lib.Tween22("synched",0);
	this.instance_3.parent = this;
	this.instance_3.setTransform(-41,81);
	this.instance_3._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance_2).to({_off:true,x:-41},8,cjs.Ease.get(1)).wait(108));
	this.timeline.addTween(cjs.Tween.get(this.instance_3).to({_off:false},8,cjs.Ease.get(1)).wait(65).to({startPosition:0},0).to({x:-193.4},3,cjs.Ease.get(1)).to({x:-267.3},4).to({_off:true},1).wait(35));

	// Layer 1
	this.instance_4 = new lib.Symbol8();
	this.instance_4.parent = this;
	this.instance_4.setTransform(223.6,39,1,1,0,0,0,68,39);

	this.timeline.addTween(cjs.Tween.get(this.instance_4).to({x:73},8,cjs.Ease.get(1)).wait(65).to({x:221.1,alpha:0},7,cjs.Ease.get(1)).to({_off:true},1).wait(35));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-370,-15,661.6,192);


(lib.Symbol7 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib.Tween18("synched",0);
	this.instance.parent = this;
	this.instance.setTransform(101.2,52.6,0.7,0.7);
	this.instance.alpha = 0;

	this.timeline.addTween(cjs.Tween.get(this.instance).to({scaleX:1,scaleY:1,x:119,y:39.5,alpha:1},7).wait(12).to({startPosition:0},0).to({scaleX:1.2,scaleY:1.2,x:99.8,y:39.6},3).to({scaleX:1,scaleY:1,x:119,y:39.5},3).wait(22).to({startPosition:0},0).to({scaleX:1.8,scaleY:1.8,x:118.9,y:39.4,alpha:0.051},8,cjs.Ease.get(1)).wait(101));

	// Layer 2
	this.instance_1 = new lib.Tween20("synched",0);
	this.instance_1.parent = this;
	this.instance_1.setTransform(134.5,86.5,0.7,0.7);
	this.instance_1.alpha = 0.051;
	this.instance_1._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance_1).wait(7).to({_off:false},0).to({scaleX:1,scaleY:1,x:83.6,y:106.4,alpha:1},7).wait(3).to({startPosition:0},0).to({scaleX:1.2,scaleY:1.2,x:57.3,y:119.9},3).to({scaleX:1,scaleY:1,x:83.6,y:106.4},3).wait(29).to({startPosition:0},0).to({scaleX:1.8,scaleY:1.8,x:65.6,y:106.3,alpha:0.051},8,cjs.Ease.get(1)).wait(96));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(38.9,25,124.6,55.3);


(lib.Symbol6 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 2
	this.instance = new lib.Tween14("synched",0);
	this.instance.parent = this;
	this.instance.setTransform(181.5,81.4,0.7,0.7);
	this.instance.alpha = 0.051;
	this.instance._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(6).to({_off:false},0).to({scaleX:1,scaleY:1,x:139.5,y:100,alpha:1},6,cjs.Ease.get(1)).wait(30).to({startPosition:0},0).to({scaleX:1.2,scaleY:1.2,x:127.5,y:112.4,alpha:0},7,cjs.Ease.get(1)).wait(107));

	// Layer 1
	this.instance_1 = new lib.Tween16("synched",0);
	this.instance_1.parent = this;
	this.instance_1.setTransform(121.5,36.5,0.7,0.7);
	this.instance_1.alpha = 0.051;

	this.timeline.addTween(cjs.Tween.get(this.instance_1).to({scaleX:1,scaleY:1,alpha:1},6,cjs.Ease.get(1)).wait(32).to({startPosition:0},0).to({scaleX:1.2,scaleY:1.2,y:31.4,alpha:0},7,cjs.Ease.get(1)).wait(111));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(23.8,10.9,195.3,51.1);


(lib.Symbol5 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib.Tween5("synched",0);
	this.instance.parent = this;
	this.instance.setTransform(123,14.5);
	this.instance.alpha = 0.031;

	this.instance_1 = new lib.Tween6("synched",0);
	this.instance_1.parent = this;
	this.instance_1.setTransform(123,14.5);

	this.timeline.addTween(cjs.Tween.get({}).to({state:[{t:this.instance}]}).to({state:[{t:this.instance_1}]},6).wait(410));
	this.timeline.addTween(cjs.Tween.get(this.instance).to({_off:true,alpha:1},6,cjs.Ease.get(1)).wait(410));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(0,0,246,29);


(lib.Symbol4 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 2
	this.instance = new lib.Tween7("synched",0);
	this.instance.parent = this;
	this.instance.setTransform(136,81.1,0.8,0.8);
	this.instance.alpha = 0.051;
	this.instance._off = true;

	this.instance_1 = new lib.Tween8("synched",0);
	this.instance_1.parent = this;
	this.instance_1.setTransform(136,65.5);
	this.instance_1._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(5).to({_off:false},0).to({_off:true,scaleX:1,scaleY:1,y:65.5,alpha:1},5,cjs.Ease.get(1)).wait(186));
	this.timeline.addTween(cjs.Tween.get(this.instance_1).wait(5).to({_off:false},5,cjs.Ease.get(1)).wait(24).to({startPosition:0},0).to({scaleX:1.5,scaleY:1.5,y:76.5,alpha:0},6,cjs.Ease.get(1)).wait(156));

	// Layer 1
	this.instance_2 = new lib.Tween9("synched",0);
	this.instance_2.parent = this;
	this.instance_2.setTransform(136,47.1,0.8,0.8);
	this.instance_2.alpha = 0.051;

	this.instance_3 = new lib.Tween10("synched",0);
	this.instance_3.parent = this;
	this.instance_3.setTransform(136,23);
	this.instance_3._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance_2).to({_off:true,scaleX:1,scaleY:1,y:23,alpha:1},5,cjs.Ease.get(1)).wait(191));
	this.timeline.addTween(cjs.Tween.get(this.instance_3).to({_off:false},5,cjs.Ease.get(1)).wait(26).to({startPosition:0},0).to({scaleX:1.5,scaleY:1.5,y:12.8,alpha:0},6,cjs.Ease.get(1)).wait(159));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(27.2,28.7,217.6,36.8);


(lib.Symbol3 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib.Tween3("synched",0);
	this.instance.parent = this;
	this.instance.setTransform(25.5,25.5);

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(14).to({startPosition:0},0).to({x:236.5,y:-176},17).wait(66).to({x:-287.8,y:109.2},0).to({x:460.6,y:-155.2},23).wait(24));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(0,0,51,51);


(lib.Symbol2 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 1
	this.instance = new lib.Tween1("synched",0);
	this.instance.parent = this;
	this.instance.setTransform(-84.5,85.5);
	this.instance.alpha = 0;

	this.instance_1 = new lib.Tween2("synched",0);
	this.instance_1.parent = this;
	this.instance_1.setTransform(68.5,85.5);
	this.instance_1._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance).to({_off:true,x:68.5,alpha:1},10).wait(495));
	this.timeline.addTween(cjs.Tween.get(this.instance_1).to({_off:false},10).wait(2).to({startPosition:0},0).to({rotation:3},3).to({rotation:0},3).wait(9).to({startPosition:0},0).to({x:-145.6,alpha:0},8).wait(470));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-153,0,137,171);


(lib.Symbol1 = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// Layer 2
	this.instance = new lib.Tween11("synched",0);
	this.instance.parent = this;
	this.instance.setTransform(150,125);
	this.instance.alpha = 0.051;
	this.instance._off = true;

	this.instance_1 = new lib.Tween12("synched",0);
	this.instance_1.parent = this;
	this.instance_1.setTransform(150,125);

	this.timeline.addTween(cjs.Tween.get({}).to({state:[]}).to({state:[{t:this.instance}]},79).to({state:[{t:this.instance_1}]},5).wait(300));
	this.timeline.addTween(cjs.Tween.get(this.instance).wait(79).to({_off:false},0).to({_off:true,alpha:1},5).wait(300));

	// Layer 1
	this.instance_2 = new lib._1();
	this.instance_2.parent = this;

	this.timeline.addTween(cjs.Tween.get(this.instance_2).wait(384));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(0,0,728,90);


// stage content:
(lib.index = function(mode,startPosition,loop) {
	this.initialize(mode,startPosition,loop,{});

	// logo
	this.instance = new lib.Symbol5("synched",0);
	this.instance.parent = this;
	this.instance.setTransform(626.9,45.5,0.7,0.7,0,0,0,123.1,14.6);
	this.instance._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance).wait(39).to({_off:false},0).wait(295));

	// t5
	this.instance_1 = new lib.Symbol10("synched",0);
	this.instance_1.parent = this;
	this.instance_1.setTransform(150.5,42.9,1,1,0,0,0,110.5,24.5);
	this.instance_1._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance_1).wait(269).to({_off:false},0).wait(65));

	// t4
	this.instance_2 = new lib.Symbol9("synched",0);
	this.instance_2.parent = this;
	this.instance_2.setTransform(218.9,46,1,1,0,0,0,68,39);
	this.instance_2._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance_2).wait(193).to({_off:false},0).to({_off:true},81).wait(60));

	// t3
	this.instance_3 = new lib.Symbol7("synched",0);
	this.instance_3.parent = this;
	this.instance_3.setTransform(150,45.5,1,1,0,0,0,89,39.5);
	this.instance_3._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance_3).wait(135).to({_off:false},0).to({_off:true},59).wait(140));

	// t2
	this.instance_4 = new lib.Symbol6("synched",0);
	this.instance_4.parent = this;
	this.instance_4.setTransform(162.3,44.5,1,1,0,0,0,139.5,36.5);
	this.instance_4._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance_4).wait(86).to({_off:false},0).to({_off:true},50).wait(198));

	// t1
	this.instance_5 = new lib.Symbol4("synched",0);
	this.instance_5.parent = this;
	this.instance_5.setTransform(291.6,24.5,1,1,0,0,0,136,23);
	this.instance_5._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance_5).wait(39).to({_off:false},0).to({_off:true},43).wait(252));

	// top
	this.instance_6 = new lib.Symbol3("synched",0);
	this.instance_6.parent = this;
	this.instance_6.setTransform(326.6,72.4,0.5,0.5,0,0,0,25.5,25.5);

	this.timeline.addTween(cjs.Tween.get(this.instance_6).to({_off:true},120).wait(214));

	// adam
	this.instance_7 = new lib.Symbol2("synched",0);
	this.instance_7.parent = this;
	this.instance_7.setTransform(291.9,45.9,0.5,0.5,0,0,0,68.6,85.5);
	this.instance_7._off = true;

	this.timeline.addTween(cjs.Tween.get(this.instance_7).wait(4).to({_off:false},0).to({_off:true},59).wait(271));

	// bg
	this.instance_8 = new lib.Symbol1("synched",0);
	this.instance_8.parent = this;
	this.instance_8.setTransform(150,125,1,1,0,0,0,150,125);

	this.timeline.addTween(cjs.Tween.get(this.instance_8).wait(334));

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(364,45,728,90);
// library properties:
lib.properties = {
	width: 728,
	height: 90,
	fps: 24,
	color: "#FFFFFF",
	opacity: 1.00,
	webfonts: {},
	manifest: [
		{src:"images/_1.png", id:"_1"},
		{src:"images/_11.png", id:"_11"},
		{src:"images/_13.png", id:"_13"},
		{src:"images/_15.png", id:"_15"},
		{src:"images/_16.png", id:"_16"},
		{src:"images/_2.png", id:"_2"},
		{src:"images/_3.png", id:"_3"},
		{src:"images/_4.png", id:"_4"},
		{src:"images/_5.png", id:"_5"},
		{src:"images/_6.png", id:"_6"},
		{src:"images/_7.png", id:"_7"},
		{src:"images/_8.png", id:"_8"},
		{src:"images/_9.png", id:"_9"},
		{src:"images/bigPizza.png", id:"bigPizza"},
		{src:"images/kola.png", id:"kola"},
		{src:"images/nefisKenar.png", id:"nefisKenar"},
		{src:"images/pzza.png", id:"pzza"}
	],
	preloads: []
};




})(lib = lib||{}, images = images||{}, createjs = createjs||{}, ss = ss||{}, AdobeAn = AdobeAn||{});
var lib, images, createjs, ss, AdobeAn;