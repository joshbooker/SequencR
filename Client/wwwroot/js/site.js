function init(val) {
    $(".dial").knob({
        'release' : function (v) { 
            window.bpmFunctions.logBpm(v);
         }
    });
}

function moveToStep(step) {
    $('.card-step').removeClass('border-primary');
    $('.card-step').removeClass('bg-secondary');
    var stepCard = $('.card-step[data-step="' + step + '"]');
    stepCard.addClass('border-primary');
    stepCard.addClass('bg-secondary');
    $('.triggerToggle[data-step="' + step + '"]:checkbox:checked').each(function(index, value) {
        playSound($(value).data('sample'));
    });
}

function playSound(sample) {
    var baseUrl = 'http://localhost:4000/media/909/';
    var mediaUrl = baseUrl + sample;
    var sound = new Howl({
        src: [mediaUrl]
    });
    sound.play();
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