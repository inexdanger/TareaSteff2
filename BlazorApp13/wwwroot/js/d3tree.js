window.d3Tree = {
    renderTree: function (containerId, data, nodoActivo) {
        d3.select("#" + containerId).selectAll("*").remove();

        const width = 600, height = 400;

        const svg = d3.select("#" + containerId)
            .append("svg")
            .attr("width", width)
            .attr("height", height);

        const root = d3.hierarchy(data);
        const treeLayout = d3.tree().size([width - 40, height - 40]);
        treeLayout(root);

        svg.selectAll('line.link')
            .data(root.links())
            .enter()
            .append('line')
            .classed('link', true)
            .attr('x1', d => d.source.x + 20)
            .attr('y1', d => d.source.y + 20)
            .attr('x2', d => d.target.x + 20)
            .attr('y2', d => d.target.y + 20)
            .attr('stroke', 'black');

        svg.selectAll('circle.node')
            .data(root.descendants())
            .enter()
            .append('circle')
            .classed('node', true)
            .attr('cx', d => d.x + 20)
            .attr('cy', d => d.y + 20)
            .attr('r', 15)
            .attr('fill', d => (nodoActivo !== undefined && d.data.name === nodoActivo) ? 'orange' : 'lightblue');

        svg.selectAll('text.label')
            .data(root.descendants())
            .enter()
            .append('text')
            .classed('label', true)
            .attr('x', d => d.x + 20)
            .attr('y', d => d.y + 20)
            .attr('dy', 5)
            .attr('text-anchor', 'middle')
            .text(d => d.data.name);
    }
};
