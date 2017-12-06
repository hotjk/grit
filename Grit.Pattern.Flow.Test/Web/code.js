var cy = cytoscape({
  container: document.getElementById('cy'),

  boxSelectionEnabled: false,
  autounselectify: true,

  layout: {
    name: 'dagre'
  },

  style: [
    {
      selector: 'node',
      style: {
        'content': 'data(id)',
        'text-opacity': 0.5,
        'text-valign': 'center',
        'text-halign': 'right',
        'background-color': '#11479e'
      }
    },

    {
      selector: 'edge',
      style: {
        'curve-style': 'bezier',
        'width': 4,
        'target-arrow-shape': 'triangle',
        'line-color': '#9dbaea',
        'target-arrow-color': '#9dbaea'
      }
    }
  ],

  elements: {
nodes: [
{ data: { id: 'Part2_1' } },{ data: { id: 'Part2_2' } },{ data: { id: 'Part2_3' } },{ data: { id: 'Part1' } },{ data: { id: 'Part4' } },{ data: { id: 'Part3_1' } },{ data: { id: 'Part3_2' } },{ data: { id: 'Part5' } },{ data: { id: 'Part3_3' } }
],
edges: [
{ data: { id: 'Part1_Part2_1', source: 'Part1', target: 'Part2_1' } },{ data: { id: 'Part1_Part2_2', source: 'Part1', target: 'Part2_2' } },{ data: { id: 'Part1_Part2_3', source: 'Part1', target: 'Part2_3' } },,{ data: { id: 'Part3_1_Part4', source: 'Part3_1', target: 'Part4' } },{ data: { id: 'Part3_2_Part4', source: 'Part3_2', target: 'Part4' } },,{ data: { id: 'Part4_Part5', source: 'Part4', target: 'Part5' } },,{ data: { id: 'Part3_2_Part4', source: 'Part3_2', target: 'Part4' } },{ data: { id: 'Part3_3_Part4', source: 'Part3_3', target: 'Part4' } },,{ data: { id: 'Part2_1_Part3_1', source: 'Part2_1', target: 'Part3_1' } },{ data: { id: 'Part2_1_Part3_2', source: 'Part2_1', target: 'Part3_2' } },{ data: { id: 'Part2_1_Part3_3', source: 'Part2_1', target: 'Part3_3' } },{ data: { id: 'Part2_2_Part3_1', source: 'Part2_2', target: 'Part3_1' } },{ data: { id: 'Part2_2_Part3_2', source: 'Part2_2', target: 'Part3_2' } },{ data: { id: 'Part2_2_Part3_3', source: 'Part2_2', target: 'Part3_3' } },{ data: { id: 'Part2_3_Part3_1', source: 'Part2_3', target: 'Part3_1' } },{ data: { id: 'Part2_3_Part3_2', source: 'Part2_3', target: 'Part3_2' } },{ data: { id: 'Part2_3_Part3_3', source: 'Part2_3', target: 'Part3_3' } },,{ data: { id: 'Part3_1_Part4', source: 'Part3_1', target: 'Part4' } },{ data: { id: 'Part3_3_Part4', source: 'Part3_3', target: 'Part4' } },
]
}
});

var bfs = cy.elements().bfs('#a', function(){}, true);

var i = 0;
var highlightNextEle = function(){
  if( i < bfs.path.length ){
    bfs.path[i].addClass('highlighted');

    i++;
    setTimeout(highlightNextEle, 1000);
  }
};

// kick off first highlight
highlightNextEle();
