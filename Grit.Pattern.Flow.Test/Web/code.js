var cy = cytoscape({
  container: document.getElementById('cy'),

  boxSelectionEnabled: false,
  autounselectify: true,

  layout: {
    name: 'dagre'
  },
  
  style: cytoscape.stylesheet()
    .selector('node')
      .css({
        'content': 'data(id)',
        'text-valign': 'center',
        'color': 'white',
        'text-outline-width': 2,
        'background-color': '#999',
        'text-outline-color': '#999'
      })
    .selector('edge')
      .css({
        'curve-style': 'bezier',
        'target-arrow-shape': 'triangle',
        'target-arrow-color': 'data(faveColor)',
        'line-color': 'data(faveColor)',
        'width': 1
      })
    .selector(':selected')
      .css({
        'background-color': 'black',
        'line-color': 'black',
        'target-arrow-color': 'black',
        'source-arrow-color': 'black'
      })
    .selector('.faded')
      .css({
        'opacity': 0.25,
        'text-opacity': 0
      }),
	  
	  

elements: {
nodes: [
{ data: { id: 'Part2_1' } },{ data: { id: 'Part2_2' } },{ data: { id: 'Part2_3' } },{ data: { id: 'Part1' } },{ data: { id: 'Part4' } },{ data: { id: 'Part3_1' } },{ data: { id: 'Part3_2' } },{ data: { id: 'Part5' } },{ data: { id: 'Part3_3' } }
],
edges: [
{ data: { source: 'Part1', target: 'Part2_1', faveColor: '#8EADDA' } },{ data: { source: 'Part1', target: 'Part2_2', faveColor: '#3BE8FC' } },{ data: { source: 'Part1', target: 'Part2_3', faveColor: '#C2A106' } },,{ data: { source: 'Part3_1', target: 'Part4', faveColor: '#6B2994' } },{ data: { source: 'Part3_2', target: 'Part4', faveColor: '#6B2994' } },,{ data: { source: 'Part4', target: 'Part5', faveColor: '#8C8A29' } },,{ data: { source: 'Part3_2', target: 'Part4', faveColor: '#985661' } },{ data: { source: 'Part3_3', target: 'Part4', faveColor: '#985661' } },,{ data: { source: 'Part2_1', target: 'Part3_1', faveColor: '#5F3DF8' } },{ data: { source: 'Part2_2', target: 'Part3_1', faveColor: '#5F3DF8' } },{ data: { source: 'Part2_3', target: 'Part3_1', faveColor: '#5F3DF8' } },{ data: { source: 'Part2_1', target: 'Part3_2', faveColor: '#927AB1' } },{ data: { source: 'Part2_2', target: 'Part3_2', faveColor: '#927AB1' } },{ data: { source: 'Part2_3', target: 'Part3_2', faveColor: '#927AB1' } },{ data: { source: 'Part2_1', target: 'Part3_3', faveColor: '#494BCF' } },{ data: { source: 'Part2_2', target: 'Part3_3', faveColor: '#494BCF' } },{ data: { source: 'Part2_3', target: 'Part3_3', faveColor: '#494BCF' } },,{ data: { source: 'Part3_1', target: 'Part4', faveColor: '#6C1543' } },{ data: { source: 'Part3_3', target: 'Part4', faveColor: '#6C1543' } },
]
}

});
