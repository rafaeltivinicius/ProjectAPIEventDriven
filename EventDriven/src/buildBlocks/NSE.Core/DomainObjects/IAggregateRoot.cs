namespace NSE.Core.DomainObjects
{
    /*
     vc tem uma entidade, mas ela pode ser filha de outra entidade e elas são persistidas juntas (tratadas como uma)
      a ideia é ter uma marcação, para dizer que tal classe, é uma AggregateRoot

     Um AGREGADO é um agrupamento de objetos associados que tratamos como uma unidade para fins de alterações 
    de dados. Cada AGREGADO possui uma raiz e um limite. O limite define o que está dentro do 
    AGREGADO. A raiz é uma ENTIDADE única e específica contida no AGREGADO.A raiz é o único membro do AGGREGATE que objetos 
    externos podem conter referências

    Um exemplo é um modelo contendo uma Customerentidade e uma Addressentidade. Nunca acessaríamos uma Addressentidade 
    diretamente do modelo, pois não faria sentido sem o contexto de um associado Customer. Então poderíamos dizer 
    que Customere Addressjuntos formam um agregado e que Customeré uma raiz agregada.
     */

    public interface IAggregateRoot
    {
    }
}
