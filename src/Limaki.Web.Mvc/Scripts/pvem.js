/** picture-viewer 
 * initializing: call "pvr([array containing array "pics" of id/src])"
 * 	possible settings
 * 		name				default	description
 *		---------------------------------------
 * 		menue_width (px)		100	static with of the menue-div (should correspond to the button- and input-widths)
 * 		pics_show_width (px)	420	default with of the pics_shows-div
 * 		button_width (px)		 40	static button width within the menue
 * 		input_width (px)		 32	static input width within the menue
*/
var picture_viewer = function(source){
/************************** navigating & scrolling **************/
	this.scroll_to = function(tonr){
		var nr = focus_pic;
		if(nr <= tonr){
			for(var i=nr;i<=tonr;i++){
				var img = document.getElementById('pic_'+pics[i].id);
				if(img){
				}else{
					var par = document.getElementById('img_'+i);
					if(pics[i]['title']){
						var title = document.createElement('div');
						title.setAttribute('class', 'pic_title');
						title.innerHTML = pics[i].title;
						par.appendChild(title);
					}
					par.appendChild(scroller(i));
				}
			}
		}else{
			for(var i=nr;i>tonr;i--){
				var img = document.getElementById('pic_'+pics[i].id);
				if(img){
				}else{
					var par = document.getElementById('img_'+i);
					par.appendChild(scroller(i));
				}
			}
		}
		var pics_show = document.getElementById('pics_shows');
		var stl = pics_show.getAttribute('style');
		if(stl == ''){
			pics_show.setAttribute('style','visibility:visible;');
 		}
		var y = 0;
		var doc = 0;
		for(var i = 0;i<=tonr;i++){
			doc = document.getElementById('img_'+i);
			doc = doc.offsetHeight;
			y = y + doc;
		}
		y = y - doc;
		pics_show.scrollTop = y;
	};
	this.scroller = function(nr){
		img = document.createElement('p');
		img.innerHTML = 'file not found: ' + pics[nr].src;
		
		img = document.createElement('img');
		
		img.setAttribute('id','pic_'+nr);
		img.setAttribute('src',pics[nr].src);
		img.setAttribute('title',pics[nr].src);
		img.setAttribute('class','pic');
		var title = pics[nr].src;
		if(pics[nr][title]){
			title = pics[nr][title];
		}
		img.setAttribute('alt',title);
		return img;
	};
	this.go_to = function(direction){
		direction = direction.getAttribute('id');
		var number = document.getElementById('number');
		var nr = parseInt(number.value);
		var tonr = null;
		switch(direction){
			case 'totop':tonr=0;break;
			case 'up':tonr=nr-1;break;
			case 'number':tonr=nr;break;
			case 'down':tonr=nr+1;break;
			case 'bottom':tonr=pics.length-1;break;
		}
		if(tonr < 0){tonr = 0;}
		if(tonr > (pics.length-1)){tonr = pics.length-1;}
		scroll_to(tonr);
		document.getElementById('pics_body').style.width = document.getElementById('pics_shows').offsetWidth + 'px';
		number.value = tonr;
		focus_pic = tonr;
	};
/************************** resizing ****************************/
	this.resizing = function(direction){
		var dir = direction.id;
		var div = document.getElementById('pics_shows');
		var x = div.offsetWidth;
		if(dir == 'tolarger'){
			x = x + x/100*10;
		}else if(dir == 'tosmaller'){ 
			x = x - x/100*10;
		}else if(dir == 'todefault'){ 
			x = pics_show_width;
		}else if(dir == 'tosmall'){
			x = x - 1;
		}else if(dir == 'tolarge'){ 
			x = x + 1;
		}
		div.setAttribute('style','visibility:visible;width:'+x+'px;');
		go_to(document.getElementById('number'));
	};
/************************** initializioning *********************/
	var init = function(source){
		/** creating the html-structure*/
		parental = document.getElementById('pics_body');
		if(parental){
		}else{
			parental = document.createElement('div');
			parental.setAttribute('id','pics_body');
			parental.setAttribute('class','pics_body');
			var top = document.getElementById('pics_er');
			if(top){
				top.removeAttribute('style');
				top.appendChild(parental);
			}else{
				document.body.appendChild(parental);
			}
		}
		if(source){
			if(source['menue_width']){
				menue_width = source['menue_width'];
			}
			if(source['pics_show_width']){
				pics_show_width = source['pics_show_width'];
			}
			if(pics_show_width<menue_width){
				pics_show_width = menue_width;
			}
			if(source['button_width']){
				button_width = source['button_width'];
			}
			if(source['input_width']){
				input_width = source['input_width'];
			}
			if(source['menue_visible']){
				menue_visible = true;
			}
			var tmp_pics = source.pics;
			pics = new Array();
			for(var i=0;i<tmp_pics.length;i++){
				var pi = tmp_pics[i];
				if(pi['src']){
					var p = new Array();
					p.id = i;
					p.src = pi.src;
					if(pi.title){
						p.title = pi.title;
					}
					pics.push(p);
				}
			}
			init_menue();
			init_shows();
			parental.style.width = menue_width + 'px';
			var number = document.getElementById('number'); 
			go_to(number);
		}
	};
	this.init_menue = function(){
		var menue = document.createElement('div');
		menue.setAttribute('id','pics_menue');
		menue.setAttribute('class','pics_menue');
		menue.style.width = menue_width + 'px';
		parental.appendChild(menue);
		init_navigation();
		init_resizing();
		if(menue_visible){
			menue.style.opacity = 1;
		}
	};
	this.init_shows = function(){
		var data = document.createElement('div');
		data.setAttribute('id','pics_data');
		data.setAttribute('class','pics_data');
		parental.appendChild(data);
		var shows = document.createElement('div');
		shows.setAttribute('id','pics_shows');
		shows.setAttribute('class','pics_shows');
		shows.setAttribute('style', 'width:'+pics_show_width+'px;');
		data.appendChild(shows);
		for(var i=0;i<pics.length;i++){
			var div = document.createElement('div');
			div.setAttribute('id','img_'+i);
			div.setAttribute('class','img');
			shows.appendChild(div);
		}
	};
	this.init_navigation = function(){
		var par = document.getElementById('pics_menue');
		parental.setAttribute('style','min-width:'+menue_width+'px;width:'+menue_width+'px;');
		init_button(par,'totop','<<','go_to(totop)','go to first entry','pics_menue_item');
		init_button(par,'up','<','go_to(up)','go to previous entry','pics_menue_item');
		var x = pics.length;
		x = x - 1;
		init_input(par,'number','0','go_to(number)','go to entry number ('+ x +' totally)','pics_menue_item');
		init_button(par,'down','>','go_to(down)','go to next entry','pics_menue_item');
		init_button(par,'bottom','>>','go_to(bottom)','go to last entry','pics_menue_item');
	};
	this.init_resizing = function(){
		var par = document.getElementById('pics_menue');
		init_button(par,'tosmaller','<--','resizing(tosmaller)','downsize to 90%','pics_menue_item');
		init_button(par,'tosmall','<','resizing(tosmall)','downsize to 99%','pics_menue_item');
		init_button(par,'todefault','||','resizing(todefault)','adjust to default','pics_menue_item');
		init_button(par,'tolarge','>','resizing(tolarge)','upsize to 101%','pics_menue_item');
		init_button(par,'tolarger','-->','resizing(tolarger)','upsize to 110%','pics_menue_item');
	};
	this.init_button = function(par,id,value,onclick,title,myclass){
		var button = document.createElement('button');
		button.setAttribute('id',id);
		button.innerHTML = value;
		button.setAttribute('onclick',onclick);
		button.setAttribute('style','width:'+button_width+'px;');
		if(title){
			button.setAttribute('title',title);
		}
		if(myclass){
			button.setAttribute('class',myclass);
		}
		par.appendChild(button);
	};
	this.init_input = function(par,id,value,onblur,title,myclass){
		var input = document.createElement('input');
		input.setAttribute('id',id);
		input.setAttribute('value',value);
		input.setAttribute('style','width:'+input_width+'px;text-align:center;');
		input.setAttribute('onblur',onblur);
		if(title){
			input.setAttribute('title',title);
		}
		if(myclass){
			input.setAttribute('class',myclass);
		}
		par.appendChild(input);
	};
	this.focus_pic = 0;
	this.parental = null;
	this.pics = null;
	this.menue_visible = false;
	this.menue_width = 360;
	this.pics_show_width = 360;
	this.button_width = 32;
	this.input_width = 32;
	this.init = new init(source);
};
