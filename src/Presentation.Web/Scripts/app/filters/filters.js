'use strict';


app.filter('partnerSearchPaging', function () {

    return function (partners, search, currentPage, enablePaging) {
        var filteredPartners = [];

        if (currentPage == null) currentPage = 1;
        if (partners == null) partners = []; // so we don't spit errors when partner is not selected
        if (search == null) {
            filteredPartners = partners;
        } else {

            partners.forEach(function(partner) {
                if ((new RegExp(search, 'i')).test(partner.Id)) {
                    filteredPartners.push(partner);
                    //console.log(partner.Id);
                } else if ((new RegExp(search, 'i')).test(partner.FirstName)) {
                    filteredPartners.push(partner);
                    //console.log(partner.FirstName);
                } else if ((new RegExp(search, 'i')).test(partner.LastName)) {
                    filteredPartners.push(partner);
                    //console.log(partner.LastName);
                }


            });
        }

        // if we want paging
        if (enablePaging) {
            // calculate and return correct page
            var startPage = currentPage - 1; // this is becuase of 0 indexing
            var startItem = startPage * 10;
            var endItem = startItem + 10; // because of 0 index

            //console.log("CurrentPartnerPage: " + currentPage + ", StartPage: " + startPage + ", startItem: " + startItem + ", endItem: " + endItem);

            return filteredPartners.slice(startItem, endItem);
        } 
        

        return filteredPartners;
        
    };

});