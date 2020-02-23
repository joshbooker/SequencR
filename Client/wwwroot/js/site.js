function init(val) {
    console.log('init');

    $(".dial").knob({
        'release' : function (v) { 
            console.log('BPM set to ' + v);
            window.bpmFunctions.logBpm(v);
         }
    });
}

function moveToStep(step) {
    console.log('moveToStep ' + step);
    $('.card-step').removeClass('border-primary');
    $('.card-step').removeClass('bg-secondary');
    var stepCard = $('.card-step[data-step="' + step + '"]');
    stepCard.addClass('border-primary');
    stepCard.addClass('bg-secondary');
}

window.bpmFunctions = {
    bpm: 120,
    dotnet: null,
    wireUp: function (dotnetHelper) {
        this.dotnet = dotnetHelper;
    },
    logBpm: function (bpm) {
        return this.dotnet
            .invokeMethodAsync('LogBpm', bpm)
                .then(r => console.log(r));
    }
};

function Channel(audio_uri) {
	this.audio_uri = audio_uri;
	this.resource = new Audio(audio_uri);
}

Channel.prototype.play = function() {
	this.resource.play();
}

function Switcher(audio_uri, num) {
	this.channels = [];
	this.num = num;
	this.index = 0;
	for (var i = 0; i < num; i++) {
		this.channels.push(new Channel(audio_uri));
	}
}

Switcher.prototype.play = function() {
	this.channels[this.index++].play();
	this.index = this.index < this.num ? this.index : 0;
}

function SoundBank() {
    var self = {};
    var urlBase = '/media/909/';

    self.playBass = function() { bass.play(); }
    self.playSnare = function() { snare.play(); }
    self.playHiTom = function() { hiTom.play(); }
    self.playMidTom = function() { midTom.play(); }
    self.playLoTom = function() { loTom.play(); }
    self.playClosed = function() { closedHat.play(); }
    self.playOpen = function() { openHat.play(); }
    self.playClap = function() { clap.play(); }

    self.init = function() {
        bass = new Switcher(urlBase + 'bass.wav', 5);
        snare = new Switcher(urlBase + 'snare.wav', 5);
        hiTom = new Switcher(urlBase + 'hi-tom.wav', 5);
        midTom = new Switcher(urlBase + 'mid-tom.wav', 5);
        loTom = new Switcher(urlBase + 'lo-tom.wav', 5);
        closedHat = new Switcher(urlBase + 'closed-hat.wav', 5);
        openHat = new Switcher(urlBase + 'open-hat.wav', 5);
        clap = new Switcher(urlBase + 'clap.wav', 5);
    }

    return self;
}

var sound = new SoundBank();
sound.init();